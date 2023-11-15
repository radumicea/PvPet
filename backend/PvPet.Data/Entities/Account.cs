namespace PvPet.Data.Entities;

public class Account : Entity
{
    public string Username { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public Pet? Pet { get; set; }
}
