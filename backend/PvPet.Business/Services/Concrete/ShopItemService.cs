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

    public async Task Restock()
    {
        var usersToBeRestocked = await _context.Set<User>().Where(u => u.SecondsToRestockShop <= 0).ToListAsync();
        var usersIds = usersToBeRestocked.Select(u => u.Id).ToHashSet();
        await _entitiesSet.Where(i => usersIds.Contains(i.UserId)).ExecuteDeleteAsync();

        foreach (var u in usersToBeRestocked)
        {
            await AddRangeAsync(Enumerable.Range(0, NumItems).Select(_ => { var i = ShopItemDto.New(); i.UserId = u.Id; return i; }));
            u.SecondsToRestockShop = 60 * 60;
            _context.Update(u);
        }

        await _context.SaveChangesAsync();
    }
}
