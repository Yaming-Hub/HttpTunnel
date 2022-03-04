using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Diagnostics;
using HttpTunnel.Models;
using Microsoft.Extensions.Logging;
using NLog;

namespace HttpTunnel.Implementations
{
    public class ForwardReceiver : IForwardReceiver
    {
        private readonly ITunnelClient requestClient;

        private readonly ILogger<ForwardReceiver> logger;

        private readonly ILoopBreaker loopBreaker;

        public ForwardReceiver(
            ITunnelClient requestClient, 
            ILoopBreaker loopBreaker, 
            ILogger<ForwardReceiver> logger)
        {
            this.requestClient = requestClient;
            this.loopBreaker = loopBreaker;
            this.logger = logger;
        }

        public async Task<ResponseData> Receive(RequestData requestData)
        {
            this.logger.LogRequestData(LogEvents.RequestReceived, requestData);

            this.loopBreaker.ValidateAndTag(requestData);

            var responseData = await this.requestClient.PostRequest(requestData);

            this.logger.LogResponseData(LogEvents.ResponseReturned, responseData);

            return responseData;
        }
    }
}
