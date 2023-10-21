using AutoMapper;
using PvPet.Business.DTOs;
using PvPet.Data.Entities;

namespace PvPet.Business.Mappings;

public class CategoryProfile : Profile
{
	public CategoryProfile()
	{
        CreateMap<Category, CategoryDto>()
                .ForAllMembers(c => c.ExplicitExpansion());
        CreateMap<CategoryDto, Category>();
    }
}
