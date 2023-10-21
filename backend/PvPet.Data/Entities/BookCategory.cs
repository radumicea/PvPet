namespace PvPet.Data.Entities;

public class BookCategory : Entity
{
    public Guid BookId { get; set; }
    public Book Book { get; set; } = default!;
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = default!;
}
