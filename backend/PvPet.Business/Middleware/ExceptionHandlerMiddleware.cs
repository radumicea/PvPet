using System.Net;
using Microsoft.AspNetCore.Http;
using PvPet.Business.Exceptions;

namespace PvPet.Business.Middleware
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
