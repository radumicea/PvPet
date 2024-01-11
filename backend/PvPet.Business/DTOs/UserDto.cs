using PvPet.Data.Entities;

namespace PvPet.Business.DTOs;

public class UserDto
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public Pet? Pet { get; set; }

    public ICollection<ShopItem>? ShopItems { get; set; }

    public int? SecondsToRestockShop { get; set; }
}
