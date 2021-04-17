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
        public BackwardReceiverMiddleware(RequestDelegate _)
        {
        }

        public async Task InvokeAsync(HttpContext context, IBackwardReceiver backwardReceiver)
        {
            var request = await RequestData.FromRequest(context.Request);

            var response = await backwardReceiver.Receive(request);

            await response.CopyTo(context.Response);
        }
    }
}
