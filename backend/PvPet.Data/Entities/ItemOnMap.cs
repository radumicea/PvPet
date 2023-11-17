namespace PvPet.Data.Entities;

public class ItemOnMap : ItemBase
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public int SecondsLeft { get; set; }
}
