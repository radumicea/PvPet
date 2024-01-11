using AutoMapper;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Mappings;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<ItemType, ItemTypeDto>()
            .ForAllMembers(i => i.ExplicitExpansion());
        CreateMap<ItemTypeDto, ItemType>();

        CreateMap<ItemOnMap, ItemOnMapDto>()
            .ForAllMembers(i => i.ExplicitExpansion());
        CreateMap<ItemOnMapDto, ItemOnMap>();

        CreateMap<ShopItem, ShopItemDto>()
            .ForAllMembers(i => i.ExplicitExpansion());
        CreateMap<ShopItemDto, ShopItem>();
    }
}
