namespace PvPet.Business.DTOs;

public class UserDto
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public PetDto? Pet { get; set; }
}
