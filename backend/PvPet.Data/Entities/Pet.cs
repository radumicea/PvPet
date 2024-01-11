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

    public Point Location { get; set; } = default!;

    public Guid UserId { get; set; }

    public User User { get; set; } = default!;
}
