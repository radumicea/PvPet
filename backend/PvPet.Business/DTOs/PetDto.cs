namespace PvPet.Business.DTOs;

public class PetDto
{
    private const int NumVariants = 9;

    public PetDto()
    {
    }

    public Guid Id { get; set; }

    public string? Name { get; set; }

    public int? Variant { get; set; }

    public int? Hp { get; set; }

    public int? Food { get; set; }

    public int? Gold { get; set; }

    public int? Attack { get; set; }

    public int? Armor { get; set; }

    public double? AttackSpeed { get; set; }

    public int? Crit { get; set; }

    public LocationDto? Location { get; set; }

    public Guid? UserId { get; set; }

    public UserDto? User { get; set; }

    public ICollection<ItemOnMapDto>? Items { get; set; }

    public static PetDto New(string name, Guid userId)
    {
        return new PetDto
        {
            Id = Guid.NewGuid(),
            Name = name,
            Variant = Random.Shared.Next(NumVariants) + 1,
            Hp = 100,
            Food = 100,
            Gold = 100,
            Attack = 10,
            Armor = 5,
            AttackSpeed = 1.0,
            Crit = 0,
            Location = new LocationDto { Latitude = 0, Longitude = 0 },
            UserId = userId
        };
    }
}
