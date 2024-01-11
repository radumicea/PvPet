namespace PvPet.Data.Entities;

public class Fight : Entity
{
    public DateTime DateTime { get; set; }

    public ICollection<PetFight> PetsFights { get; set; } = null!;

    public ICollection<Pet> Pets { get; set; } = null!;

    public ICollection<FightRound> Rounds { get; set; } = null!;
}
