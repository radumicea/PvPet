namespace PvPet.Business.DTOs;

public class PetDto
{
    private static readonly string[] variants = { "Slime", "Golem" };

    public PetDto(string name, Guid userId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Variant = variants[Random.Shared.Next(variants.Length)];
        Hp = 100;
        Food = 100;
        Starvation = 0;
        Attack = 10;
        Armor = 5;
        AttackSpeed = 1.0;
        Crit = 0;
        UserId = userId;
    }

    public PetDto()
    {
    }

    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Variant { get; set; }

    public int? Hp { get; set; }

    public int? Food { get; set; }

    public int? Starvation { get; set; }

    public int? Attack { get; set; }

    public int? Armor { get; set; }

    public double? AttackSpeed { get; set; }

    public int? Crit { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public Guid? UserId { get; set; }

    public UserDto? User { get; set; }

    public ICollection<ItemDto>? Items { get; set; }
}
