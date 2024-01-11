using AutoMapper;
using Crop360.Business.Services.Generic;
using Microsoft.EntityFrameworkCore;
using PvPet.Business.DTOs;
using PvPet.Business.Services.Contracts;
using PvPet.Data.Entities;

namespace PvPet.Business.Services.Concrete;

public class ShopItemService : BaseService<ShopItem, ShopItemDto>, IShopItemService
{
    private const int NumItems = 10;

    public ShopItemService(DbContext context, IMapper mapper) : base(context, mapper)
    {
    }


    public async Task Restock(int tickInSeconds)
    {
        await _context.Set<Pet>().ExecuteUpdateAsync(p => p.SetProperty(p => p.SecondsToRestockShop, p => p.SecondsToRestockShop - tickInSeconds));

        var petsToBeRestocked = await _context.Set<Pet>().Where(u => u.SecondsToRestockShop <= 0).ToListAsync();
        var petsIds = petsToBeRestocked.Select(p => p.Id).ToHashSet();
        await _entitiesSet.Where(i => petsIds.Contains(i.PetId)).ExecuteDeleteAsync();

        foreach (var p in petsToBeRestocked)
        {
            await AddRangeAsync(Enumerable.Range(0, NumItems).Select(_ => { var i = ShopItemDto.New(); i.PetId = p.Id; return i; }));
            p.SecondsToRestockShop = 60 * 60;
            _context.Update(p);
        }

        await _context.SaveChangesAsync();
    }
}
