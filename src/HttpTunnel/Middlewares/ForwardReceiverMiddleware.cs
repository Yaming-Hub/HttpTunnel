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
    /// The forward sender takes incoming request and sends them to remote dispatcher.
    /// </summary>
    public class ForwardReceiverMiddleware
    {
        public ForwardReceiverMiddleware(RequestDelegate _)
        {
        }

        public async Task InvokeAsync(HttpContext context, IForwardReceiver forwardReceiver)
        {
            var request = await RequestData.FromRequest(context.Request);

            var response = await forwardReceiver.Receive(request);

            await response.CopyTo(context.Response);
        }
    }
}
