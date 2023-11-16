namespace PvPet.Data.Entities;

public class Item : Entity
{
    public int Attack { get; set; }

    public int Armor { get; set; }

    public double AttackSpeed { get; set; }

    public int Crit { get; set; }

    public Guid PetId { get; set; }

    public Pet Pet { get; set; } = default!;
}
