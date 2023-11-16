using PvPet.Business.Exceptions;
using System.Net;

namespace PvPet.API.Middleware
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                if (e is ResponseStatusException exception)
                    context.Response.StatusCode = (int)exception.StatusCode;
                else context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsync(e.ToString());
            }
        }
    }
}
