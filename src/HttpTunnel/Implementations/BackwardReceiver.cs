using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Diagnostics;
using HttpTunnel.Models;
using Microsoft.Extensions.Logging;

namespace HttpTunnel.Implementations
{
    public class BackwardReceiver : IBackwardReceiver
    {
        private readonly IBackwardRequestHandler backwardRequestHandler;

        private readonly ILogger<BackwardReceiver> logger;

        public BackwardReceiver(IBackwardRequestHandler backwardRequestHandler, ILogger<BackwardReceiver> logger)
        {
            this.backwardRequestHandler = backwardRequestHandler;
            this.logger = logger;
        }

        public async Task<ResponseData> Receive(RequestData requestData)
        {
            this.logger.LogRequestData(LogEvents.RequestReceived, requestData);

            var responseData = await this.backwardRequestHandler.Handle(requestData);

            this.logger.LogResponseData(LogEvents.ResponseReturned, responseData);

            return responseData;
        }
    }
}
