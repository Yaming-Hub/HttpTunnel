using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HttpTunnel.Configurations;
using HttpTunnel.Contracts;
using HttpTunnel.Models;
using Microsoft.Extensions.Configuration;

namespace HttpTunnel.Implementations
{
    public class BackwardSender : SenderBase, IBackwardSender
    {
        private readonly IRequestClient requestClient;

        public BackwardSender(IRequestClient requestClient, IConfiguration configuration)
            : base(configuration)
        {
            this.requestClient = requestClient;
        }

        public void Send(RequestData requestData)
        {
            _ = SendAsync(requestData);
        }

        protected override Redirect[] GetRedirects(IConfiguration configuration) => configuration.GetForwardConfiguration().Redirects;

        private async Task SendAsync(RequestData requestData)
        {
            var responseData = await this.InternalSend(requestData);

            await this.requestClient.PutResponse(requestData.Id, responseData);
        }
    }
}
