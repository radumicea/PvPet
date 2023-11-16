using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPet.Business.Extensions
{
    using Microsoft.AspNetCore.Http;
    using System.Linq;

    public static class HttpContextExtensions
    {
        public static string GetAccount(this HttpContext context)
        {
            return context.User.Claims.FirstOrDefault()?.Value;
        }
    }
}
