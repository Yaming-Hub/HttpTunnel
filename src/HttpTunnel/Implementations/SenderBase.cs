using System;
using System.Net.Http;
using System.Threading.Tasks;
using HttpTunnel.Configurations;
using HttpTunnel.Models;
using Microsoft.Extensions.Configuration;

namespace HttpTunnel.Implementations
{
    public abstract class SenderBase
    {
        private readonly HttpClient httpClient;

        private readonly UrlReplaceRule[] replaceRules;

        protected SenderBase(IConfiguration configuration)
        {
            this.httpClient = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = ServerCertificateValidation.TrustAll
            });

            this.replaceRules = this.GetReplaceRules(configuration);
        }

        protected async Task<ResponseData> InternalSend(RequestData requestData)
        {
            var request = requestData.ToRequest();

            // Redirect request based on rules
            var originalUri = request.RequestUri;
            var replacedUri = originalUri.Replace(this.replaceRules);
            if (!originalUri.Equals(replacedUri))
            {
                Console.WriteLine($"Replace Url: {originalUri} -> {replacedUri}");
            }

            request.RequestUri = replacedUri;

            var response = await this.httpClient.SendAsync(request);

            return await ResponseData.FromResponse(response);
        }

        protected abstract UrlReplaceRule[] GetReplaceRules(IConfiguration configuration);
    }
}
