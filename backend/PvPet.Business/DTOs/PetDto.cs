namespace PvPet.Business.DTOs;

public class PetDto
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Variant { get; set; }

    public int? Hp { get; set; }

    public int? Food { get; set; }

    public int? Starvation { get; set; }

    public int? Attack { get; set; }

    public int? Armor { get; set; }

    public int? AttackSpeed { get; set; }

    public int? Crit { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public Guid? AccountId { get; set; }

    public AccountDto? Account { get; set; }
}
