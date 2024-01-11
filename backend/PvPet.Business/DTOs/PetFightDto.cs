namespace PvPet.Business.DTOs;

public class PetFightDto
{
    public Guid Id { get; set; }

    public Guid? FightId { get; set; }

    public Guid? PetId { get; set; }

    public bool? Winner { get; set; }

    public PetDto? Pet { get; set; }

    public FightDto? Fight { get; set; }
}