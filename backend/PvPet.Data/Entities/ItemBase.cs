using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

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

[JsonConverter(typeof(StringEnumConverter))]
public enum ItemType
{
    gold,

    armor,

    food,

    potion,

    weapon
}