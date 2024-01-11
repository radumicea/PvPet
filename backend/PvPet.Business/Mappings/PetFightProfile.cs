using AutoMapper;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Mappings;

public class PetFightProfile : Profile
{
    public PetFightProfile()
    {
        CreateMap<PetFight, PetFightDto>()
            .ForAllMembers(i => i.ExplicitExpansion());
        CreateMap<PetFightDto, PetFight>();
    }
}
