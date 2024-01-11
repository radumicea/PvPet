using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;

namespace PvPet.API.BackgroundServices;

public class GameLoopBackgroundService : BackgroundService
{
    private const int SecondsInTick = 5;
    private static readonly TimeSpan Tick = TimeSpan.FromSeconds(SecondsInTick);
    private const int FightRange = 20;
    private const int PickupRange = 3;
    private const int NrItemsOnMap = 1000;

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
        var itemOnMapService = scope.ServiceProvider.GetRequiredService<IItemOnMapService>();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var shopItemService = scope.ServiceProvider.GetRequiredService<IShopItemService>();

        (var winners, var losers) = await CommenceFights(petService);
        var items = await UpdateItemsOnMap(itemOnMapService);
        await userService.UpdateRestockTime(SecondsInTick);
        await shopItemService.Restock();
    }

    private static async Task<(List<Guid> Winners, List<Guid> Losers)> CommenceFights(IPetService petService)
    {
        var winners = new List<Guid>();
        var losers = new List<Guid>();

        var pairs = await petService.GetClosestPairsInRange(FightRange);
        foreach ((var p1, var p2) in pairs)
        {
            (var winner, var loser) = await Fight(p1, p2, petService);
            winners.Add(winner);
            losers.Add(loser);
        }

        return (winners, losers);
    }

    private static async Task<List<ItemOnMapDto>> UpdateItemsOnMap(IItemOnMapService itemOnMapService)
    {
        var pairs = await itemOnMapService.GetItemsWithClosestPetInRange(PickupRange);
        foreach ((var item, var pet) in pairs)
        {
            pet.Attack += item.Attack;
            pet.Crit += item.Crit;
            pet.Armor += item.Armor;
            pet.AttackSpeed += item.AttackSpeed;
            pet.Hp += item.Hp;
            pet.Food += item.Food;
            pet.Gold += item.Gold;
        }

        var remaining = await itemOnMapService
            .UpdateAvailability(SecondsInTick, pairs.Select(pair => pair.Item1.Id).ToHashSet());

        var newItems = Enumerable
            .Range(0, NrItemsOnMap - remaining)
            .Select(_ => ItemOnMapDto.New());

        await itemOnMapService.AddRangeAsync(newItems);

        return (await itemOnMapService.QueryAsync()).ToList();
    }

    private static async Task<(Guid Winner, Guid Loser)> Fight(PetDto p1, PetDto p2, IPetService petService)
    {
        for (var i = 0; i < 5 && p1.Hp > 0 && p2.Hp > 0; i++)
        {
            if (Random.Shared.NextDouble() < p1.AttackSpeed / (p1.AttackSpeed + p2.AttackSpeed))
            {
                var dmg = p1.Attack - p2.Armor;
                if (Random.Shared.NextDouble() < p1.Crit / 100.0)
                    dmg *= 2;
                p2.Hp -= Math.Max(dmg!.Value, 0);
            }
            else
            {
                var dmg = p2.Attack - p1.Armor;
                if (Random.Shared.NextDouble() < p2.Crit / 100.0)
                    dmg *= 2;
                p2.Hp -= Math.Max(dmg!.Value, 0);
            }
        }

        if (p1.Hp > p2.Hp)
        {
            p1.Gold += p2.Gold / 2;
            p2.Gold -= p2.Gold / 2;
        }

        else
        {
            p2.Gold += p1.Gold / 2;
            p1.Gold -= p1.Gold / 2;
        }

        await petService.UpdateRangeAsync(new[] { p1, p2 });

        return p1.Hp > p2.Hp
            ? (p1.Id, p2.Id)
            : (p2.Id, p1.Id);
    }
}
