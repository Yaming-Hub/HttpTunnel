using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace HttpTunnel.Middlewares
{
    /// <summary>
    /// This middle ware will return status for Is-Alive requests.
    /// </summary>
    public class StatusMiddleware
    {
        public const string TunnelIsAliveHeader = "x-tunnel-status";

        private readonly RequestDelegate next;
        private readonly string name;

        public StatusMiddleware(RequestDelegate next, string name)
        {
            this.next = next;
            this.name = name;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(TunnelIsAliveHeader))
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsync(this.name);
            }
            else
            {
                await this.next(context);
            }
        }
    }
}
