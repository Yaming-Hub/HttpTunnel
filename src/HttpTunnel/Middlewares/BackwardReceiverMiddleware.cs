using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Models;
using Microsoft.AspNetCore.Http;

namespace HttpTunnel.Middlewares
{
    /// <summary>
    /// The backward receiver takes backward requests and push them into queue.
    /// </summary>
    public class BackwardReceiverMiddleware
    {
        private readonly RequestDelegate next;

        public BackwardReceiverMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, IBackwardReceiver backwardReceiver)
        {
            var path = context.Request.Path.Value;
            if (path != null && path.ToLower().StartsWith("/tunnel"))
            {
                // Ignore tunner internal requests.
                await this.next(context);
            }
            else
            {
                var request = RequestData.FromRequest(context.Request);

                var response = await backwardReceiver.Receive(request);

                response.CopyTo(context.Response);
            }
        }
    }
}
