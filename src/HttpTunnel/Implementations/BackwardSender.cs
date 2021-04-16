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
    public class BackwardSender : IBackwardSender
    {
        private readonly ITunnelClient tunnelClient;

        private readonly HttpClient httpClient;

        private readonly Redirect[] redirects;

        public BackwardSender(ITunnelClient tunnelClient, IConfiguration configuration)
        {
            this.tunnelClient = tunnelClient;

            this.httpClient = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = ServerCertificateValidation.TrustAll
            });

            this.redirects = configuration.GetClientConfiguration().Redirects;
        }

        public void Send(RequestData requestData)
        {
            _ = SendAsync(requestData);
        }

        private async Task SendAsync(RequestData requestData)
        {
            var request = requestData.ToRequest();

            var response = await this.httpClient.SendAsync(request);

            var responseData = ResponseData.FromResponse(response);

            await this.tunnelClient.PutResponse(requestData.Id, responseData);
        }
    }
}
