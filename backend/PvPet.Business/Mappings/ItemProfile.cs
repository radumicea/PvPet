using AutoMapper;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Mappings;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemDto>()
            .ForAllMembers(i => i.ExplicitExpansion());
        CreateMap<ItemDto, Item>();
    }
}
