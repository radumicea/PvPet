using AutoMapper;
using PvPet.Business.DTOs;
using PvPet.Business.Services;
using PvPet.Business.Services.Contracts;

namespace PvPet.API.BackgroundServices;

public class GameLoopBackgroundService : BackgroundService
{
    private const int SecondsInTick = 5;
    private static readonly TimeSpan Tick = TimeSpan.FromSeconds(SecondsInTick);
    private const int FightRange = 20;
    private const int PickupRange = 3;
    private const int NrItemsOnMap = 1000;
    private const int NumRounds = 5;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GameLoopBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var timer = new PeriodicTimer(Tick);

        while (!cancellationToken.IsCancellationRequested && await timer.WaitForNextTickAsync(cancellationToken))
        {
            await DoWorkAsync();
        }
    }

    private async Task DoWorkAsync()
    {
        await using var scope = _serviceScopeFactory.CreateAsyncScope();

        var petService = scope.ServiceProvider.GetRequiredService<IPetService>();
        var fightService = scope.ServiceProvider.GetRequiredService<IFightService>();
        var itemOnMapService = scope.ServiceProvider.GetRequiredService<IItemOnMapService>();
        var shopItemService = scope.ServiceProvider.GetRequiredService<IShopItemService>();
        var petItemService = scope.ServiceProvider.GetRequiredService<IPetItemService>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();

        await CommenceFights(petService, fightService, notificationService);
        var items = await UpdateItemsOnMap(itemOnMapService, petItemService, mapper, notificationService);
        await shopItemService.Restock(SecondsInTick);
        await petService.MakePetsHungry();
    }

    private static async Task CommenceFights(IPetService petService, IFightService fightService, NotificationService notificationService)
    {
        var pairs = await petService.GetClosestPairsInRange(FightRange, SecondsInTick);
        foreach ((var p1, var p2) in pairs)
        {
            await Fight(p1, p2, petService, fightService, notificationService);
        }
    }

    private static async Task<List<ItemOnMapDto>> UpdateItemsOnMap(IItemOnMapService itemOnMapService, IPetItemService petItemService, IMapper mapper, NotificationService notificationService)
    {
        var pairs = await itemOnMapService.GetItemsWithClosestPetInRange(PickupRange);
        var petItemsToAdd = new List<PetItemDto>();

        foreach ((var item, var pet) in pairs)
        {
            var petItem = mapper.Map<PetItemDto>(item);
            petItem.PetId = pet.Id;
            petItemsToAdd.Add(petItem);

            await notificationService.NotifyPetOwner(pet.Id, "Your pet just found a new item!");

            pet.Attack += item.Attack;
            pet.Crit += item.Crit;
            pet.Armor += item.Armor;
            pet.AttackSpeed += item.AttackSpeed;
            pet.Hp += item.Hp;
            pet.Food += item.Food;
            pet.Gold += item.Gold;
        }

        await petItemService.AddRangeAsync(petItemsToAdd);

        var remaining = await itemOnMapService
            .UpdateAvailability(SecondsInTick, pairs.Select(pair => pair.Item1.Id).ToHashSet());

        var newItems = Enumerable
            .Range(0, NrItemsOnMap - remaining)
            .Select(_ => ItemOnMapDto.New());

        await itemOnMapService.AddRangeAsync(newItems);

        return (await itemOnMapService.QueryAsync()).ToList();
    }

    private static async Task Fight(PetDto p1, PetDto p2, IPetService petService, IFightService fightService, NotificationService notificationService)
    {
        var fight = new FightDto
        {
            Id = Guid.NewGuid(),
            DateTime = DateTime.UtcNow
        };

        var petFightOne = new PetFightDto
        {
            Id = Guid.NewGuid(),
            FightId = fight.Id,
            PetId = p1.Id
        };

        var petFightTwo = new PetFightDto
        {
            Id = Guid.NewGuid(),
            FightId = fight.Id,
            PetId = p2.Id
        };

        for (var i = 0; i < NumRounds && p1.Hp > 0 && p2.Hp > 0; i++)
        {
            var round = new FightRoundDto
            {
                Id = Guid.NewGuid(),
                FightId = fight.Id,
                XStartHp = p1.Hp,
                YStartHp = p2.Hp
            };

            if (Random.Shared.NextDouble() < p1.AttackSpeed / (p1.AttackSpeed + p2.AttackSpeed))
            {
                var dmg = p1.Attack - p2.Armor;
                if (Random.Shared.NextDouble() < p1.Crit / 100.0)
                    dmg *= 2;
                p2.Hp -= Math.Max(dmg!.Value, 0);

                round.XDamage = dmg;
                round.YDamage = 0;
            }
            else
            {
                var dmg = p2.Attack - p1.Armor;
                if (Random.Shared.NextDouble() < p2.Crit / 100.0)
                    dmg *= 2;
                p2.Hp -= Math.Max(dmg!.Value, 0);

                round.YDamage = dmg;
                round.XDamage = 0;
            }
        }

        p1.CooldownSeconds = 60 * 30;
        p2.CooldownSeconds = 60 * 30;

        if (p1.Hp > p2.Hp)
        {
            p1.Gold += p2.Gold / 2;
            p2.Gold -= p2.Gold / 2;

            await notificationService.NotifyPetOwner(p1.Id, "Your pet just won a fight!");
            await notificationService.NotifyPetOwner(p2.Id, "Your pet just lost a fight...");
            petFightOne.Winner = true;
        }

        else
        {
            p2.Gold += p1.Gold / 2;
            p1.Gold -= p1.Gold / 2;

            await notificationService.NotifyPetOwner(p2.Id, "Your pet just won a fight!");
            await notificationService.NotifyPetOwner(p1.Id, "Your pet just lost a fight...");
            petFightTwo.Winner = true;
        }

        await petService.UpdateRangeAsync(new[] { p1, p2 });
        await petService.DeleteRangeAsync(new[] { p1, p2 }.Where(p => p.Hp <= 0));
        await fightService.AddAsync(fight);
    }
}
