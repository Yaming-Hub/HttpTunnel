using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace BotServer
{
    /// <summary>
    /// This class defines a middle where which simply returns the request body back.
    /// </summary>
    public class PongMiddleware
    {
        public PongMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sb = new StringBuilder(1024);
            sb.Append(context.Request.Method);
            sb.Append(" ").Append(context.Request.GetDisplayUrl());
            string body = null;
            if (context.Request.Body != null)
            {
                using (var sr = new StreamReader(context.Request.Body))
                {
                    body = await sr.ReadToEndAsync();

                    sb.Append(" ").Append(body);
                }
            }

            Console.WriteLine(sb.ToString());

            context.Response.StatusCode = StatusCodes.Status200OK;
            if (body != null)
            {
                await context.Response.WriteAsync(body);
            }
        }
    }
}
