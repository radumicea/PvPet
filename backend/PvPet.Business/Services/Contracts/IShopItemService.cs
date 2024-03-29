﻿using Crop360.Business.Services.Generic;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Services.Contracts;

public interface IShopItemService : IBaseService<ShopItem, ShopItemDto>
{
    Task Restock(int secondsInTick);

    Task<IEnumerable<ShopItem>> GetShopItems(Guid petId);

    Task<ShopItem?> GetShopItemById(Guid id);
    Task RemoveItem(ShopItem item);
}
