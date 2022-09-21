using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore_webapi2
{
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// IApplicationBuilder: Define una clase que provee mecanismos para config un pipeline de requests
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCustomLogger(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomLogger>();
        }
    }
}
