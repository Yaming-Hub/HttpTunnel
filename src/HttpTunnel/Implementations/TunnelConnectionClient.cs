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

namespace HttpTunnel.Implementations
{
    public class TunnelConnectionClient : ITunnelConnectionClient
    {
        private readonly ITunnelClient tunnelClient;
        private readonly IBackwardSender backwardSender;

        public TunnelConnectionClient(ITunnelClient tunnelClient, IBackwardSender backwardSender)
        {
            this.tunnelClient = tunnelClient;
            this.backwardSender = backwardSender;
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            while (true)
            {
                try
                {
                    using (var stream = await this.tunnelClient.Connect())
                    using (var sr = new StreamReader(stream))
                    {
                        while (!sr.EndOfStream)
                        {
                            var json = await sr.ReadLineAsync();

                            try
                            {
                                var requestData = JsonSerializer.Deserialize<RequestData>(json);
                                this.backwardSender.Send(requestData);
                            }
                            catch (JsonException)
                            {
                                // Ignore back requests for now.
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());

                    // Wait a short period of time before retry.
                    await Task.Delay(TimeSpan.FromSeconds(15));
                }
            }
        }
    }
}
