namespace PvPet.Data.Entities;

public class User : Entity
{
    public string? FirebaseToken { get; set; }

    public string Username { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;

    public Pet? Pet { get; set; }
}
