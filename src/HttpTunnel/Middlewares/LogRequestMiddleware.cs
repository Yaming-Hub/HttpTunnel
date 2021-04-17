using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace HttpTunnel.Middlewares
{
    /// <summary>
    /// The backward receiver takes backward requests and push them into queue.
    /// </summary>
    public class LogRequestMiddleware
    {
        private readonly RequestDelegate next;

        public LogRequestMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            var r = context.Request;
            Console.WriteLine($"{r.Scheme}://{r.Host}/{r.Path}");
            Console.WriteLine($"{r.GetDisplayUrl()}");

            return this.next(context);
        }
    }
}
