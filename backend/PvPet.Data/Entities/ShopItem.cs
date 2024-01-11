namespace PvPet.Data.Entities;

public class ShopItem : ItemBase
{
    public int Price { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
}
