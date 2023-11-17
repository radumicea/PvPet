namespace PvPet.Business.DTOs;

public class ItemDto
{
    public Guid Id { get; set; }

    public int? Attack { get; set; }

    public int? Armor { get; set; }

    public double? AttackSpeed { get; set; }

    public int? Crit { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public int? SecondsLeft { get; set; }

    public Guid? PetId { get; set; }

    public PetDto? Pet { get; set; }
}
