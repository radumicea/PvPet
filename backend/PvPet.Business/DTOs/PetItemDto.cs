namespace PvPet.Business.DTOs;

public class PetItemDto : ItemBaseDto
{
    public Guid? PetId { get; set; }

    public PetDto? Pet { get; set; }
}
