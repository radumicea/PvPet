using AutoMapper;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Mappings;

public class ItemOnMapProfile : Profile
{
	public ItemOnMapProfile()
	{
        CreateMap<ItemOnMap, ItemDto>()
            .ForAllMembers(bc => bc.ExplicitExpansion());
        CreateMap<ItemDto, ItemOnMap>();
    }
}
