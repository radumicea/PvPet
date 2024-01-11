namespace PvPet.Business.DTOs;

public class FightDto
{
    public Guid Id { get; set; }

    public DateTime? DateTime { get; set; }

    public ICollection<PetFightDto>? PetsFights { get; set; }

    public ICollection<FightRoundDto>? Rounds { get; set; }
}

public class FightRoundDto
{
    public Guid Id { get; set; }

    public int? XStartHp { get; set; }

    public int? YStartHp { get; set; }

    public int? XDamage { get; set; }

    public int? YDamage { get; set; }

    public Guid? FightId { get; set; }

    public FightDto? Fight { get; set; }
}
