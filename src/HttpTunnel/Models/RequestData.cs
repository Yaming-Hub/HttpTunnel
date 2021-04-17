using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace HttpTunnel.Models
{
    public class RequestData
    {
        private static int LastRequestId = 1;

        public int Id { get; set; }

        public string Method { get; set; }

        public string Uri { get; set; }

        public List<HeaderData> Headers { get; set; }

        public string Body { get; set; }

        public static async Task<RequestData> FromRequest(HttpRequest request)
        {
            var headers = new List<HeaderData>(request.Headers.Count);
            foreach (var header in request.Headers)
            {
                foreach (var value in header.Value)
                {
                    headers.Add(new HeaderData
                    {
                        Name = header.Key,
                        Value = value
                    });
                }
            }

            string body = null;
            if (request.Body != null)
            {
                body = await Base64StreamReader.ReadStreamAsBase64String(request.Body);
            }

            int id = Interlocked.Increment(ref LastRequestId);

            return new RequestData
            {
                Id = id,
                Method = request.Method,
                Uri = request.GetDisplayUrl(),
                Headers = headers,
                Body = body
            };
        }

        public HttpRequestMessage ToRequest()
        {
            var request = new HttpRequestMessage(new HttpMethod(this.Method), this.Uri);

            foreach (var header in this.Headers)
            {
                request.Headers.TryAddWithoutValidation(header.Name, header.Value);
            }

            if (this.Body != null)
            {
                var bytes = Convert.FromBase64String(this.Body);
                request.Content = new ByteArrayContent(bytes);
            }

            return request;
        }

        private static int GetDefaultPort(string scheme)
        {
            if ("https".Equals(scheme, StringComparison.OrdinalIgnoreCase))
            {
                return 443;
            }
            else
            {
                return 80;
            }
        }
    }
}
