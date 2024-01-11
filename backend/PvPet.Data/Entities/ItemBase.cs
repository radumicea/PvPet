namespace PvPet.Data.Entities;

public abstract class ItemBase : Entity
{
    public ItemType Type { get; set; }

    public int Pictocode { get; set; }

    public int Attack { get; set; }

    public int Armor { get; set; }

    public double AttackSpeed { get; set; }

    public int Crit { get; set; }

    public int Hp { get; set; }

    public int Food { get; set; }

    public int Gold { get; set; }
}

public enum ItemType
{
    Gold, Armor, Food, Potion, Weapon
}