using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace HttpTunnel.Middlewares
{
    /// <summary>
    /// This middle ware will return status for Is-Alive requests.
    /// </summary>
    public class ModeMiddleware
    {
        public const string TunnelIsAliveHeader = "x-tunnel-mode";

        private readonly RequestDelegate next;
        private readonly string mode;

        public ModeMiddleware(RequestDelegate next, string mode)
        {
            this.next = next;
            this.mode = mode;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(TunnelIsAliveHeader))
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsync(this.mode);
            }
            else
            {
                await this.next(context);
            }
        }
    }
}
