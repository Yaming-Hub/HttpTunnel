using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HttpTunnel.Configurations;
using Microsoft.Extensions.Configuration;

namespace HttpTunnel.Implementations
{
    public abstract class ClientBase
    {
        protected HttpClient HttpClient { get; }

        protected ClientBase(IConfiguration configuration)
        {
            var clientConfiguration = configuration.GetForwardConfiguration();

            this.HttpClient = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = ServerCertificateValidation.TrustAll
            });

            this.BaseUri = $"https://{clientConfiguration.TunnelHost}:{clientConfiguration.TunnelPort}/tunnel";
        }

        protected string BaseUri { get; }
    }
}
