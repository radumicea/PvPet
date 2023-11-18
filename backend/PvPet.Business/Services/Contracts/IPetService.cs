using Crop360.Business.Services.Generic;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Services.Contracts;

public interface IPetService : IBaseService<Pet, PetDto>
{
    Task<IEnumerable<(PetDto, PetDto)>> GetClosestPairsInRange(double range);
}
