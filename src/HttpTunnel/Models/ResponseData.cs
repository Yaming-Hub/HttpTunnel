﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace HttpTunnel.Models
{
    public class ResponseData
    {
        public int StatusCode { get; set; }

        public List<HeaderData> Headers { get; set; }

        public string Body { get; set; }

        public static ResponseData FromResponse(HttpResponseMessage response)
        {
            var headers = new List<HeaderData>();
            foreach (var header in response.Headers)
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
            if (response.Content != null)
            {
                var responseStream = response.Content.ReadAsStream();
                body = Base64StreamReader.ReadStreamAsBase64String(responseStream);
            }

            return new ResponseData
            {
                StatusCode = (int)response.StatusCode,
                Headers = headers,
                Body = body
            };
        }


        public void CopyTo(HttpResponse response)
        {
            response.StatusCode = this.StatusCode;

            foreach (var header in this.Headers.GroupBy(x => x.Name, x => x.Value))
            {
                response.Headers[header.Key] = new StringValues(header.ToArray());
            }

            var bytes = Convert.FromBase64String(this.Body);
            using (var memoryStream = new MemoryStream(bytes))
            {
                memoryStream.CopyTo(response.Body);
            }
        }
    }
}
