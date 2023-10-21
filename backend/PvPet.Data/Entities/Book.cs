namespace PvPet.Data.Entities;

public class Book : Entity
{
    public string Title { get; set; } = default!;
    public ICollection<BookCategory> BookCategories { get; } = new List<BookCategory>();
}
