namespace PvPet.Data.Entities;

public class Pet : Entity
{
    public string Name { get; set; } = default!;

    public string Variant { get; set; } = default!;

    public int Hp { get; set; }

    public int Food { get; set; }

    public int Starvation { get; set; }

    public int Attack { get; set; }

    public int Armor { get; set; }

    public double AttackSpeed { get; set; }

    public int Crit { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public Guid AccountId { get; set; }

    public Account Account { get; set; } = default!;

    public ICollection<Item> Items { get; set; } = default!;
}
