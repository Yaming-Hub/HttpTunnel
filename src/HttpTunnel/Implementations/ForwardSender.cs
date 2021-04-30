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
    public class ForwardSender : SenderBase, IForwardSender
    {
        private readonly ILogger<ForwardSender> logger;

        public ForwardSender(IConfiguration configuration, ILogger<ForwardSender> logger)
            : base(configuration, logger)
        {
            this.logger = logger;
        }

        public async Task<ResponseData> Send(RequestData requestData)
        {
            this.logger.LogRequestData(LogEvents.RequestSent, requestData);

            var responseData = await this.InternalSend(requestData);

            this.logger.LogResponseData(LogEvents.ResponseReceived, responseData);

            return responseData;
        }

        protected override UrlReplaceRule[] GetReplaceRules(IConfiguration configuration)
            => configuration.GetBackwardConfiguration().UrlReplaceRules;
    }
}
