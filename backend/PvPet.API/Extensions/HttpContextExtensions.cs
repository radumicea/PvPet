namespace PvPet.API.Extensions;

public static class HttpContextExtensions
{
    public static string GetUsername(this HttpContext context)
    {
        return context.User.Claims.First().Value;
    }
}