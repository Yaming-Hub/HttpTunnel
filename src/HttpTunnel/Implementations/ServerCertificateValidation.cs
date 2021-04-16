using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using HttpTunnel.Configurations;
using HttpTunnel.Models;
using Microsoft.Extensions.Configuration;


namespace HttpTunnel.Implementations
{
    public static class ServerCertificateValidation
    {
        public static bool TrustAll(HttpRequestMessage m, X509Certificate2 cert, X509Chain chain, SslPolicyErrors errs) => true;

    }
}
