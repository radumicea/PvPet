using NetTopologySuite.Geometries;

namespace PvPet.Data.Entities;

public class Pet : Entity
{
    public string Name { get; set; } = default!;

    public int Variant { get; set; }

    public int Hp { get; set; }

    public int Food { get; set; }

    public int Gold { get; set; }

    public int Attack { get; set; }

    public int Armor { get; set; }

    public double AttackSpeed { get; set; }

    public int Crit { get; set; }

    public int CooldownSeconds { get; set; }

    public Point Location { get; set; } = default!;

    public int SecondsToRestockShop { get; set; }

    public Guid UserId { get; set; }

    public ICollection<PetItem> Items { get; set; } = null!;

    public ICollection<ShopItem> ShopItems { get; set; } = null!;

    public User User { get; set; } = default!;

    public ICollection<Fight> Fights { get; set; } = null!;
    public ICollection<PetFight> PetsFights { get; set; } = null!;
}
