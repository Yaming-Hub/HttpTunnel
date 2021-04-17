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
    public class TunnelClient : ClientBase, ITunnelClient
    {
        public TunnelClient(IConfiguration configuration)
            : base(configuration)
        {
            this.HttpClient.Timeout = TimeSpan.FromMinutes(15);
        }

        public async Task<Stream> Connect()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{this.BaseUri}/connect");
            var response = await this.HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();
        }
    }
}
