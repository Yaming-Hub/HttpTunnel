using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HttpTunnel.Configurations;

namespace HttpTunnel.Implementations
{
    public static class HttpRequestMessageExtensions
    {
        public static void Redirect(this HttpRequestMessage request, IEnumerable<Redirect> redirects)
        {
            foreach (var redirect in redirects)
            {
                if (redirect.From == null || redirect.To == null)
                {
                    // Bad configuration.
                    continue;
                }

                if (IsMatch(request, redirect))
                {
                    var uriBuilder = new UriBuilder(request.RequestUri);
                    uriBuilder.Port = redirect.To.Port;
                    if (!string.IsNullOrEmpty(redirect.From.Path) && !string.IsNullOrEmpty(redirect.To.Path))
                    {
                        string remaining = uriBuilder.Path.Substring(redirect.From.Path.Length);
                        uriBuilder.Path = redirect.To.Path + remaining;
                    }

                    request.RequestUri = uriBuilder.Uri;
                }
            }
        }

        private static bool IsMatch(HttpRequestMessage request, Redirect redirect)
        {
            var uri = request.RequestUri;

            if (uri.Port != redirect.From.Port)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(redirect.From.Path))
            {
                if (string.IsNullOrEmpty(uri.AbsolutePath))
                {
                    return false;
                }

                if (!uri.AbsolutePath.StartsWith(redirect.From.Path))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
