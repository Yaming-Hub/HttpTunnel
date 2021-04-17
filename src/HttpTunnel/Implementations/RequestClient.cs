using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HttpTunnel.Configurations;
using HttpTunnel.Contracts;
using HttpTunnel.Models;
using Microsoft.Extensions.Configuration;

namespace HttpTunnel.Implementations
{
    public class RequestClient : ClientBase, IRequestClient
    {
        public RequestClient(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ResponseData> PostRequest(RequestData requestData)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{this.BaseUri}/requests");
            var requestBody = JsonSerializer.Serialize(requestData);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await this.HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            var responseData = JsonSerializer.Deserialize<ResponseData>(responseBody);
            return responseData;
        }

        public async Task PutResponse(int requestId, ResponseData responseData)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{this.BaseUri}/responses/{requestId}");
            var requestBody = JsonSerializer.Serialize(responseData);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await this.HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
    }
}
