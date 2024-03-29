﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public string ContentType { get; set; }

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
                if (request.ContentType.IsTextContent())
                {
                    using (var sr = new StreamReader(request.Body))
                    {
                        body = await sr.ReadToEndAsync();
                    }
                }
                else
                {
                    body = await Base64StreamReader.ReadStreamAsBase64String(request.Body);
                }
            }

            int id = Interlocked.Increment(ref LastRequestId);

            return new RequestData
            {
                Id = id,
                Method = request.Method,
                Uri = request.GetDisplayUrl(),
                Headers = headers,
                Body = body,
                ContentType = request.ContentType
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
                if (this.ContentType.IsTextContent())
                {
                    request.Content = new StringContent(this.Body);
                }
                else
                {
                    var bytes = Convert.FromBase64String(this.Body);
                    request.Content = new ByteArrayContent(bytes);
                }

                if (this.ContentType != null)
                {
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(this.ContentType);
                }
            }

            return request;
        }
    }
}
