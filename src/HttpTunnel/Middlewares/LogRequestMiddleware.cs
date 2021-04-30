using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace HttpTunnel.Middlewares
{
    /// <summary>
    /// The backward receiver takes backward requests and push them into queue.
    /// </summary>
    public class LogRequestMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string name;

        public LogRequestMiddleware(RequestDelegate next, string name)
        {
            this.next = next;
            this.name = name;
        }

        public Task InvokeAsync(HttpContext context, ILogger<LogRequestMiddleware> logger)
        {
            var r = context.Request;
            logger.LogInformation($"{this.name}: {r.Method} {r.GetDisplayUrl()}");

            return this.next(context);
        }
    }
}
