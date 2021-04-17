﻿using System.Net.Http;
using System.Threading.Tasks;
using HttpTunnel.Configurations;
using HttpTunnel.Contracts;
using HttpTunnel.Models;
using Microsoft.Extensions.Configuration;

namespace HttpTunnel.Implementations
{
    public class ForwardSender : SenderBase, IForwardSender
    {
        public ForwardSender(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ResponseData> Send(RequestData requestData)
        {
            return await this.InternalSend(requestData);
        }

        protected override Redirect[] GetRedirects(IConfiguration configuration) => configuration.GetBackwardConfiguration().Redirects;
    }
}