namespace PvPet.API.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetAccountId(this HttpContext context)
    {
        // return context.User.Claims.FirstOrDefault()?.Value;
        // Until we implement auth on client side:
        return Guid.Parse("3B5625EB-CC25-4F97-A3FB-8BBFB76BCD14");
    }
}