namespace PvPet.Data.Entities;

public class User : Entity
{
    public string Username { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public Pet Pet { get; set; } = default!;

    public ICollection<ShopItem> ShopItems { get; set; } = null!;

    public int SecondsToRestockShop { get; set; }
}
