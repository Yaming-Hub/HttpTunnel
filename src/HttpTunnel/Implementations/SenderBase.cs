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

        private readonly Redirect[] redirects;

        protected SenderBase(IConfiguration configuration)
        {
            this.httpClient = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = ServerCertificateValidation.TrustAll
            });

            this.redirects = this.GetRedirects(configuration);
        }

        protected async Task<ResponseData> InternalSend(RequestData requestData)
        {
            var request = requestData.ToRequest();

            // Redirect request if necessary.
            request.Redirect(this.redirects);

            var response = await this.httpClient.SendAsync(request);

            return await ResponseData.FromResponse(response);
        }

        protected abstract Redirect[] GetRedirects(IConfiguration configuration);
    }
}
