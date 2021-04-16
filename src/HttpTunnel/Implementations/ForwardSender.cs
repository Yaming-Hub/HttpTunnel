using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Models;

namespace HttpTunnel.Implementations
{
    public class ForwardSender : IForwardSender
    {
        private readonly HttpClient httpClient;

        public ForwardSender()
        {
            this.httpClient = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = ServerCertificateValidation.TrustAll
            });
        }

        public async Task<ResponseData> Send(RequestData requestData)
        {
            var request = requestData.ToRequest();

            var response = await this.httpClient.SendAsync(request);

            return ResponseData.FromResponse(response);
        }
    }
}
