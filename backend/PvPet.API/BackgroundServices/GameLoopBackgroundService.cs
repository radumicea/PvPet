using PvPet.API.Extensions;
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
        var itemService = scope.ServiceProvider.GetRequiredService<IItemService>();

        (var winners, var losers) = await CommenceFights(petService);
        var items = await UpdateItemsOnMap(itemOnMapService, itemService);
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

    private static async Task<List<ItemDto>> UpdateItemsOnMap(IItemOnMapService itemOnMapService, IItemService itemService)
    {
        var pairs = await itemOnMapService.GetItemsWithClosestPetInRange(PickupRange);
        foreach ((var item, var pet) in pairs)
        {
            item.PetId = pet.Id;
            await itemService.AddAsync(item);
        }

        var remaining = await itemOnMapService
            .UpdateAvailability(SecondsInTick, pairs.Select(pair => pair.Item1.Id).ToHashSet());

        // TO DO
        var newItems = Enumerable.Range(0, NrItemsOnMap - remaining).Select(_ => new ItemDto
        {
            Attack = 1,
            Armor = 0,
            AttackSpeed = 0,
            Crit = 0,
            Location = new LocationDto
            {
                Latitude = Random.Shared.NextDouble(46.17, 46.18),
                Longitude = Random.Shared.NextDouble(21.3, 21.4)
            },
            SecondsLeft = Random.Shared.Next(300, 3600)
        }).ToList();

        await itemOnMapService.AddRangeAsync(newItems);

        return (await itemOnMapService.QueryAsync()).ToList();
    }

    private static async Task<(Guid Winner, Guid Loser)> Fight(PetDto p1, PetDto p2, IPetService petService)
    {
        // TO DO
        if (Random.Shared.Next() % 2 == 0)
        {
            return (p1.Id, p2.Id);
        }
        else
        {
            return (p2.Id, p1.Id);
        }
    }
}
