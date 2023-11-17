namespace PvPet.API.Extensions;

public static class MiscExtensions
{
    public static double NextDouble(this Random rand, double min, double max)
    {
        return rand.NextDouble() * (max - min) + min;
    }
}
