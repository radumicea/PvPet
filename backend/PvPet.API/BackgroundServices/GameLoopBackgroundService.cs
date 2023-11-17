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
        var pets = (await petService.QueryAsync()).ToList();

        var itemOnMapService = scope.ServiceProvider.GetRequiredService<IItemOnMapService>();
        var itemsOnMap = (await itemOnMapService.QueryAsync()).ToList();
        var itemService = scope.ServiceProvider.GetRequiredService<IItemService>();

        (var winners, var losers) = await CommenceFights(pets, petService);
        var items = await UpdateItemsOnMap(itemsOnMap, itemOnMapService, itemService, pets);
    }

    private static async Task<(List<Guid> Winners, List<Guid> Losers)> CommenceFights(List<PetDto> pets, IPetService petService)
    {
        var winners = new List<Guid>();
        var losers = new List<Guid>();

        for (var i = 0; i < pets.Count - 1; i++)
        {
            for (var j = i + 1; j < pets.Count; j++)
            {
                var p1 = pets[i];
                var p2 = pets[j];

                if (ComputeDistance(p1.Latitude!.Value, p2.Latitude!.Value, p1.Longitude!.Value, p2.Longitude!.Value) < FightRange)
                {
                    (var winner, var loser) = await Fight(p1, p2, petService);
                    winners.Add(winner);
                    losers.Add(loser);
                }
            }
        }

        return (winners, losers);
    }

    private static async Task<List<ItemDto>> UpdateItemsOnMap(
        List<ItemDto> itemsOnMap,
        IItemOnMapService itemOnMapService,
        IItemService itemService,
        List<PetDto> pets
        )
    {
        var n = itemsOnMap.Count;

        foreach (var item in itemsOnMap)
        {
            item.SecondsLeft -= SecondsInTick;
         
            var isTaken = false;

            foreach (var pet in pets)
            {
                if (ComputeDistance(item.Latitude!.Value, pet.Latitude!.Value, item.Longitude!.Value, pet.Longitude!.Value) <= PickupRange)
                {
                    isTaken = true;
                    item.PetId = pet.Id;
                    await itemService.AddAsync(item);
                    break;
                }
            }

            if (isTaken || item.SecondsLeft <= 0)
            {
                await itemOnMapService.DeleteAsync(item);
                n--;
            }
            else
            {
                await itemOnMapService.UpdateAsync(item);
            }
        }

        var newItems = Enumerable.Range(0, NrItemsOnMap - n).Select(_ => new ItemDto
        {
            Attack = 1,
            Armor = 0,
            AttackSpeed = 0,
            Crit = 0,
            Latitude = Random.Shared.NextDouble(46.17, 46.18),
            Longitude = Random.Shared.NextDouble(21.3, 21.4),
            SecondsLeft = Random.Shared.Next(300, 3600)
        }).ToList();

        await itemOnMapService.AddRangeAsync(newItems);

        return (await itemOnMapService.QueryAsync()).ToList();
    }

    private static double ComputeDistance(double lat1, double lat2, double lon1, double lon2)
    {
        // Earth's mean radius in meter
        var R = 6371e3;
        // convert to radiant
        var φ1 = lat1 * Math.PI / 180;
        var φ2 = lat2 * Math.PI / 180;
        var Δφ = (lat2 - lat1) * Math.PI / 180;
        var Δλ = (lon2 - lon1) * Math.PI / 180;

        var sinHalfΔφ = Math.Sin(Δφ / 2);
        var sinHalfΔλ = Math.Sin(Δλ / 2);

        var a = sinHalfΔφ * sinHalfΔφ + Math.Cos(φ1) * Math.Cos(φ2) * sinHalfΔλ * sinHalfΔλ;
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        // In meteres
        return R * c;
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
