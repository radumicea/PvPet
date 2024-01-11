using PvPet.Data.Entities;

namespace PvPet.Business.DTOs;

public class ShopItemDto : ItemBaseDto
{
    public int? Price { get; set; }

    public Guid? UserId { get; set; }

    public User? User { get; set; }

    public static ShopItemDto New()
    {
        var item = ItemBaseDto.New<ShopItemDto>();
        item.Price = Random.Shared.Next(50, 5000);

        return item;
    }
}
