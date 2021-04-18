using System;
using System.Threading.Tasks;
using HttpTunnel.Configurations;
using HttpTunnel.Contracts;
using HttpTunnel.Models;
using Microsoft.Extensions.Configuration;

namespace HttpTunnel.Implementations
{
    public class BackwardSender : SenderBase, IBackwardSender
    {
        private readonly ITunnelClient requestClient;

        public BackwardSender(ITunnelClient requestClient, IConfiguration configuration)
            : base(configuration)
        {
            this.requestClient = requestClient;
        }

        public void Send(RequestData requestData)
        {
            _ = SendAsync(requestData);
        }

        protected override UrlReplaceRule[] GetReplaceRules(IConfiguration configuration)
            => configuration.GetForwardConfiguration().UrlReplaceRules;

        private async Task SendAsync(RequestData requestData)
        {
            var responseData = await this.InternalSend(requestData);
            Console.WriteLine($"Respone Received for request {requestData.Id}, StatusCode={responseData.StatusCode}.");

            try
            {
                await this.requestClient.PutResponse(requestData.Id, responseData);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
