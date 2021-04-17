using System.Net.Http;
using System.Text;
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
        public TunnelClient(IConfiguration configuration)
        {
            var clientConfiguration = configuration.GetForwardConfiguration();

            this.HttpClient = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = ServerCertificateValidation.TrustAll
            });

            this.BaseUri = $"https://{clientConfiguration.TunnelHost}:{clientConfiguration.TunnelPort}/tunnel";
        }

        private HttpClient HttpClient { get; }

        private string BaseUri { get; }

        public async Task<ResponseData> PostRequest(RequestData requestData)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{this.BaseUri}/requests");
            var requestBody = JsonSerializer.Serialize(requestData);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await this.HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ResponseData>(responseBody);
        }

        public async Task<RequestData> GetRequest()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{this.BaseUri}/requests");

            var response = await this.HttpClient.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<RequestData>(responseBody);
        }

        public async Task PutResponse(int requestId, ResponseData responseData)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{this.BaseUri}/responses/{requestId}");
            var requestBody = JsonSerializer.Serialize(responseData);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await this.HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
