namespace PvPet.Data.Entities;

public class Item : ItemBase
{
    public Guid PetId { get; set; }

    public Pet Pet { get; set; } = default!;
}
