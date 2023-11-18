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

    public async Task<IEnumerable<(PetDto, PetDto)>> GetClosestPairsInRange(double range)
    {
        var query =
            from p1 in _entitiesSet
            from p2 in _entitiesSet.Where(p2 => p1.Id != p2.Id && p1.Location.Distance(p2.Location) <= range)
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

        return result;
    }
}
