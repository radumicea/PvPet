namespace PvPet.Business.DTOs;

public class BookCategoryDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public BookDto? Book { get; set; }
    public Guid CategoryId { get; set; }
    public CategoryDto? Category { get; set; }
}
