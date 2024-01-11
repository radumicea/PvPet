using AutoMapper;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Mappings;

public class FightProfile : Profile
{
    public FightProfile()
    {
        CreateMap<Fight, FightDto>()
            .ForAllMembers(i => i.ExplicitExpansion());
        CreateMap<FightDto, Fight>();

        CreateMap<FightRound, FightRoundDto>()
            .ForAllMembers(i => i.ExplicitExpansion());
        CreateMap<FightRoundDto, FightRound>();
    }
}
