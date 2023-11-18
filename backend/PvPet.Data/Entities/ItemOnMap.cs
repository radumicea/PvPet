using NetTopologySuite.Geometries;

namespace PvPet.Data.Entities;

public class ItemOnMap : ItemBase
{
    public Point Location { get; set; } = default!;

    public int SecondsLeft { get; set; }
}
