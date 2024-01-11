using PvPet.Business.Services;
using PvPet.Business.Services.Concrete;
using PvPet.Business.Services.Contracts;

namespace PvPet.API.Configurations;

public static class ServicesSettingsExtension
{
    public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
    {
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IPetService, PetService>();
        services.AddTransient<IItemOnMapService, ItemOnMapService>();
        services.AddTransient<IShopItemService, ShopItemService>();
        services.AddTransient<IFightService, FightService>();
        services.AddTransient<IPetItemService, PetItemService>();
        services.AddTransient<NotificationService>();

        return services;
    }
}
