namespace PvPet.Business.DTOs;

public class BookDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public ICollection<BookCategoryDto> BookCategories { get; } = new List<BookCategoryDto>();
}
