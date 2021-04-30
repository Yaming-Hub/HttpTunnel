using System;
using System.Net.Http;
using System.Threading.Tasks;
using HttpTunnel.Configurations;
using HttpTunnel.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HttpTunnel.Implementations
{
    public abstract class SenderBase
    {
        private readonly HttpClient httpClient;

        private readonly UrlReplaceRule[] replaceRules;

        private readonly ILogger logger;

        protected SenderBase(IConfiguration configuration, ILogger logger)
        {
            this.httpClient = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = ServerCertificateValidation.TrustAll
            });

            this.replaceRules = this.GetReplaceRules(configuration);
            this.logger = logger;
        }

        protected async Task<ResponseData> InternalSend(RequestData requestData)
        {
            var request = requestData.ToRequest();

            // Redirect request based on rules
            var originalUri = request.RequestUri;
            var replacedUri = originalUri.Replace(this.replaceRules);
            if (!originalUri.Equals(replacedUri))
            {
                this.logger.LogTrace($"Replace Url: {originalUri} -> {replacedUri}");
            }

            request.RequestUri = replacedUri;

            var response = await this.httpClient.SendAsync(request);

            return await ResponseData.FromResponse(response);
        }

        protected abstract UrlReplaceRule[] GetReplaceRules(IConfiguration configuration);
    }
}
