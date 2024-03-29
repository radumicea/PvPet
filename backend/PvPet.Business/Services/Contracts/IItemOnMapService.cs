﻿using Crop360.Business.Services.Generic;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Services.Contracts
{
    public interface IItemOnMapService : IBaseService<ItemOnMap, ItemOnMapDto>
    {
        Task<IEnumerable<(ItemOnMapDto, PetDto)>> GetItemsWithClosestPetInRange(double range);
        Task<int> UpdateAvailability(int elapsedSeconds, HashSet<Guid> collected);
    }
}
