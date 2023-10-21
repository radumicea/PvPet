using AutoMapper;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Mappings;

public class BookCategoryProfile : Profile
{
	public BookCategoryProfile()
	{
        CreateMap<BookCategory, BookCategoryDto>()
                .ForAllMembers(bc => bc.ExplicitExpansion());
        CreateMap<BookCategoryDto, BookCategory>();
    }
}
