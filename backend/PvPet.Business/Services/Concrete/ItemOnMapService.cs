using AutoMapper;
using Crop360.Business.Services.Generic;
using Microsoft.EntityFrameworkCore;
using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;
using PvPet.Data.Entities;

namespace PvPet.Business.Services.Concrete;

public class ItemOnMapService : BaseService<ItemOnMap, ItemDto>, IItemOnMapService
{
    public ItemOnMapService(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<IEnumerable<(ItemDto, PetDto)>> GetItemsWithClosestPetInRange(double range)
    {
        var query =
            from item in _entitiesSet
            from pet in _context.Set<Pet>().Where(pet => pet.Location.Distance(item.Location) <= range)
            orderby pet.Location.Distance(item.Location)
            select new { item, pet };

        var pairs = await query.ToListAsync();
        var taken = new HashSet<Guid>();

        var result = new List<(ItemDto, PetDto)>();

        foreach (var pair in pairs)
        {
            if (taken.Contains(pair.item.Id))
                continue;

            result.Add((_mapper.Map<ItemDto>(pair.item), _mapper.Map<PetDto>(pair.pet)));

            taken.Add(pair.item.Id);
        }

        return result;
    }

    public async Task<int> UpdateAvailability(int elapsedSeconds, HashSet<Guid> collected)
    {
        await _entitiesSet.ExecuteUpdateAsync(s => s.SetProperty(i => i.SecondsLeft, i => i.SecondsLeft - elapsedSeconds));
        await _entitiesSet.Where(i => i.SecondsLeft <= 0 || collected.Contains(i.Id)).ExecuteDeleteAsync();

        return await _entitiesSet.CountAsync();
    }
}
