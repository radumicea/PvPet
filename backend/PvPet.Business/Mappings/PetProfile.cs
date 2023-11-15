using AutoMapper;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Mappings;

public class PetProfile : Profile
{
    public PetProfile()
    {
        CreateMap<Pet, PetDto>()
            .ForAllMembers(c => c.ExplicitExpansion());
        CreateMap<PetDto, Pet>();
    }
}
