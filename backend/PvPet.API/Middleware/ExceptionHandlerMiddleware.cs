using PvPet.Business.Exceptions;
using System.Net;

namespace PvPet.API.Middleware
{
    public class ExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

                await next(context);
        }
    }
}
