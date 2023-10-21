namespace PvPet.Business.DTOs;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public ICollection<BookCategoryDto> BookCategories { get; } = new List<BookCategoryDto>();
}
