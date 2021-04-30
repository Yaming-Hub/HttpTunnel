using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Models;
using Microsoft.Extensions.Logging;

namespace HttpTunnel.Implementations
{
    public class RequestPuller : IRequestPuller
    {
        private readonly ITunnelClient tunnelClient;
        private readonly IBackwardSender backwardSender;

        private readonly ILogger<RequestPuller> logger;

        public RequestPuller(ITunnelClient tunnelClient, IBackwardSender backwardSender, ILogger<RequestPuller> logger)
        {
            this.tunnelClient = tunnelClient;
            this.backwardSender = backwardSender;
            this.logger = logger;
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            while (true)
            {
                try
                {
                    var requestData = await this.tunnelClient.GetRequest();
                    if (requestData != null)
                    {
                        this.logger.LogInformation($"Got backward request: [{requestData.Id}] {requestData.Method} {requestData.Uri}");
                        this.backwardSender.Send(requestData);
                    }
                }
                catch (Exception e)
                {
                    this.logger.LogError(e, "Fail to connect to tunnel server");

                    // Wait a short period of time before retry.
                    await Task.Delay(TimeSpan.FromSeconds(15));
                }
            }
        }
    }
}
