namespace PvPet.Data.Entities;

public class Category : Entity
{
    public string Name { get; set; } = default!;
    public ICollection<BookCategory> BookCategories { get; } = new List<BookCategory>();
}
