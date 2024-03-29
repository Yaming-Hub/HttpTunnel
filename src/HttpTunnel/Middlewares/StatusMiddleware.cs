﻿using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HttpTunnel.Configurations;
using HttpTunnel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;

namespace HttpTunnel.Middlewares
{
    /// <summary>
    /// This middle ware will return status for Is-Alive requests.
    /// </summary>
    public class StatusMiddleware
    {
        public const string TunnelIsAliveHeader = "x-tunnel-status";

        private static readonly RuntimeInfo Runtime;

        private readonly RequestDelegate next;
        private readonly ExecutionMode mode;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        static StatusMiddleware()
        {
            StatusMiddleware.Runtime = new RuntimeInfo
            {
                ProcessId = Process.GetCurrentProcess().Id,
                ApplicationDirectory = Path.GetDirectoryName(typeof(StatusMiddleware).Assembly.Location),
            };
        }

        public StatusMiddleware(RequestDelegate next, ExecutionMode mode)
        {
            this.next = next;
            this.mode = mode;

            this.jsonSerializerOptions = new JsonSerializerOptions
            {
                Converters ={
                    new JsonStringEnumConverter()
                }
            };
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            if (context.Request.Headers.ContainsKey(TunnelIsAliveHeader))
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                var status = new TunnelStatus
                {
                    Mode = this.mode,
                    Runtime = StatusMiddleware.Runtime,
                    ForwardConfiguration = configuration.GetForwardConfiguration(),
                    BackwardConfiguration = configuration.GetBackwardConfiguration(),
                };

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(status, this.jsonSerializerOptions));
                await context.Response.CompleteAsync();
            }
            else
            {
                await this.next(context);
            }
        }
    }
}
