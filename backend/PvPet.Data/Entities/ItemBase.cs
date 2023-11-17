namespace PvPet.Data.Entities;

public abstract class ItemBase : Entity
{
    public int Attack { get; set; }

    public int Armor { get; set; }

    public double AttackSpeed { get; set; }

    public int Crit { get; set; }
}
