namespace PvPet.Data.Entities;

public class PetFight : Entity
{
    public Guid FightId { get; set; }

    public Guid PetId { get; set; }

    public bool Winner { get; set; }

    public Pet Pet { get; set; } = null!;

    public Fight Fight { get; set; } = null!;
}
