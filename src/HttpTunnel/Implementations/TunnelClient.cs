using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HttpTunnel.Configurations;
using HttpTunnel.Contracts;
using HttpTunnel.Models;
using Microsoft.Extensions.Configuration;

namespace HttpTunnel.Implementations
{
    public class TunnelClient : ITunnelClient
    {
        private readonly HttpClient dataClient;
        private readonly HttpClient connectionClient;

        private readonly TunnelClientConfiguration clientConfiguration;

        public TunnelClient(IConfiguration configuration)
        {
            this.clientConfiguration = configuration.GetClientConfiguration();

            this.dataClient = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = ServerCertificateValidation.TrustAll
            });

            this.connectionClient = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = ServerCertificateValidation.TrustAll
            })
            {
                Timeout = TimeSpan.FromDays(1)
            };
        }

        private string TunnelBaseUri => $"{this.clientConfiguration.TunnelHost}:{this.clientConfiguration.TunnelPort}/tunnel";

        public async Task<Stream> Connect()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{this.TunnelBaseUri}/connect");
            var response = await this.connectionClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();
        }

        public async Task<ResponseData> PostRequest(RequestData requestData)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{this.TunnelBaseUri}/requests");
            var requestBody = JsonSerializer.Serialize(requestData);
            request.Content = new StringContent(requestBody);

            var response = await this.dataClient.SendAsync(request);
            if (response.StatusCode != System.Net.HttpStatusCode.Processing)
            {
                throw new HttpRequestException($"Unexpected http status code {response.StatusCode}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();

            var responseData = JsonSerializer.Deserialize<ResponseData>(responseBody);
            return responseData;
        }

        public async Task PutResponse(int requestId, ResponseData responseData)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{this.TunnelBaseUri}/responses/{requestId}");
            var requestBody = JsonSerializer.Serialize(responseData);
            request.Content = new StringContent(requestBody);

            var response = await this.dataClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
