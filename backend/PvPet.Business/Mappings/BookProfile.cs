using AutoMapper;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Mappings;

public class BookProfile : Profile
{
	public BookProfile()
	{
        CreateMap<Book, BookDto>()
                .ForAllMembers(b => b.ExplicitExpansion());
        CreateMap<BookDto, Book>();
    }
}
