namespace PvPet.Data.Entities;

public class PetItem : ItemBase
{
    public Guid PetId { get; set; }

    public Pet Pet { get; set; } = null!;
}
