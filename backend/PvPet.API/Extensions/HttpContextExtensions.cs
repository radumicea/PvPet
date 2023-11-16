namespace PvPet.API.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetAccountId(this HttpContext context)
    {
        // return context.User.Claims.FirstOrDefault()?.Value;
        // Until we implement auth on client side:
        return Guid.Parse("4F2BD20E-2B7C-460C-B338-BA774AE18E74");
    }
}