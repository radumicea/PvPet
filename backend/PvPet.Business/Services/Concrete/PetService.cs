using AutoMapper;
using Crop360.Business.Services.Generic;
using Microsoft.EntityFrameworkCore;
using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;
using PvPet.Data.Entities;

namespace PvPet.Business.Services.Concrete;

public class PetService : BaseService<Pet, PetDto>, IPetService
{
    public PetService(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public override async Task<bool> DeleteAsync(PetDto pet)
    {
        // TODO Send death notification
        return await base.DeleteAsync(pet);
    }

    public override async Task<bool> DeleteRangeAsync(IEnumerable<PetDto> pets)
    {
        // TODO Send death notification
        return await base.DeleteRangeAsync(pets);
    }

    public async Task<IEnumerable<(PetDto, PetDto)>> GetClosestPairsInRange(double range, int secondsInTick)
    {
        var query =
            from p1 in _entitiesSet.Where(p => p.CooldownSeconds == 0)
            from p2 in _entitiesSet.Where(p2 => p2.CooldownSeconds == 0 && p2.Id > p1.Id && p2.Location.Distance(p1.Location) <= range)
            orderby p1.Location.Distance(p2.Location)
            select new { p1, p2 };

        var pairs = await query.ToListAsync();
        var taken = new HashSet<Guid>();

        var result = new List<(PetDto, PetDto)>();

        foreach (var pair in pairs)
        {
            if (taken.Contains(pair.p1.Id) || taken.Contains(pair.p2.Id))
                continue;

            result.Add((_mapper.Map<PetDto>(pair.p1), _mapper.Map<PetDto>(pair.p2)));

            taken.Add(pair.p1.Id);
            taken.Add(pair.p2.Id);
        }

        await _entitiesSet.ExecuteUpdateAsync(p => p.SetProperty(p => p.CooldownSeconds, p => Math.Max(0, p.CooldownSeconds - secondsInTick)));

        return result;
    }

    public async Task MakePetsHungry()
    {
        var pets = await _entitiesSet.ToListAsync();

        foreach (var p in pets)
        {
            var oneInOneHundred = Random.Shared.Next(100) == 0;

            // It will take on average 5 (tickInSeconds) * 100 (petHp) / (1 / 100) (chance to decrement) == 13.89 hours for pet to starve to death
            if (oneInOneHundred)
            {
                p.Food--;
                _context.Update(p);
            }
        }

        await _context.SaveChangesAsync();

        await DeleteRangeAsync(await QueryAsync(predicate: p => p.Food <= 0));
    }
}
