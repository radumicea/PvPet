namespace PvPet.Data.Entities;

public class FightRound
{
    public Guid Id { get; set; }

    public int XStartHp { get; set; }

    public int YStartHp { get; set; }

    public int XDamage { get; set; }

    public int YDamage {  get; set; }

    public Guid FightId { get; set; }

    public Fight Fight { get; set; } = null!;
}
