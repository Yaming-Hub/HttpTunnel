using System;
using System.Text.Json;
using System.Threading.Tasks;
using HttpTunnel.Configurations;
using HttpTunnel.Contracts;
using HttpTunnel.Diagnostics;
using HttpTunnel.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HttpTunnel.Implementations
{
    public class BackwardSender : SenderBase, IBackwardSender
    {
        private readonly ITunnelClient requestClient;

        private readonly ILogger<BackwardSender> logger;

        public BackwardSender(ITunnelClient requestClient, IConfiguration configuration, ILogger<BackwardSender> logger)
            : base(configuration, logger)
        {
            this.requestClient = requestClient;
            this.logger = logger;
        }

        public void Send(RequestData requestData)
        {
            _ = SendAsync(requestData);
        }

        protected override UrlReplaceRule[] GetReplaceRules(IConfiguration configuration)
            => configuration.GetForwardConfiguration().UrlReplaceRules;

        private async Task SendAsync(RequestData requestData)
        {
            this.logger.LogRequestData(LogEvents.RequestSent, requestData);

            var responseData = await this.InternalSend(requestData);

            this.logger.LogResponseData(LogEvents.ResponseReceived, responseData);

            try
            {
                await this.requestClient.PutResponse(requestData.Id, responseData);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "PutResponse failed");
            }
        }
    }
}
