using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using HttpTunnel.Contracts;
using HttpTunnel.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace HttpTunnel.Middlewares
{
    /// <summary>
    /// The backward receiver takes backward requests and push them into queue.
    /// </summary>
    public class ConnectionServerMiddleware
    {
        private readonly RequestDelegate next;

        public ConnectionServerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConnectionServer connectionServer)
        {
            if (IsConnectRequest(context.Request))
            {
                context.Response.StatusCode = StatusCodes.Status102Processing;

                var feature = new StreamResponseBodyFeature(context.Response.Body);

                // Each connection will last for 5 minutes.
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(300));

                using (var sw = new StreamWriter(context.Response.Body))
                {
                    await connectionServer.Run(sw, cancellationTokenSource.Token);
                }

                // Note, this will be a long running operation, and the connection server should handle
                // duplicate connection request internal.
                
            }
            else
            {
                await this.next(context);
            }
        }

        private static bool IsConnectRequest(HttpRequest request)
        {
            if (request.Method.ToLower() != "get")
            {
                return false;
            }

            if (request.Path.Value == null ||
                request.Path.Value.ToLower() != "/tunnel/connect")
            {
                return false;
            }

            return true;
        }
    }
}
