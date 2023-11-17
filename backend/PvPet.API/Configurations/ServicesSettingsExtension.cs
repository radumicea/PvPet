using PvPet.Business.Services.Concrete;
using PvPet.Business.Services.Contracts;

namespace PvPet.API.Configurations;

public static class ServicesSettingsExtension
{
    public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddTransient<IBookService, BookService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IBookCategoryService, BookCategoryService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IPetService, PetService>();
        services.AddTransient<IItemOnMapService, ItemOnMapService>();
        services.AddTransient<IItemService, ItemService>();

        return services;
    }
}
