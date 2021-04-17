using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Models;

namespace HttpTunnel.Implementations
{
    public class TunnelConnectionServer : ITunnelConnectionServer
    {
        private readonly object lockObject;
        private readonly AsyncQueue<RequestData> queue;

        public TunnelConnectionServer()
        {
            this.lockObject = new object();
            this.queue = new AsyncQueue<RequestData>();
        }

        public async Task Run(StreamWriter writer, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Try to wait 15 seconds until receives one request.
                    var requestData = await this.queue.Dequeue(TimeSpan.FromSeconds(15), cancellationToken);
                    WriteRequest(writer, requestData);
                }
                catch (TaskCanceledException e) when (e.CancellationToken == cancellationToken)
                {
                    // The 15 seconds timeout exceeded, ignore and continue.
                }
            }
        }

        public void SendRequest(RequestData request)
        {
            this.queue.Enqueue(request);
        }

        private static void WriteRequest(StreamWriter writer, RequestData requestData)
        {
            var json = JsonSerializer.Serialize(requestData);
            writer.WriteLine(json);
            writer.Flush();
        }
    }
}
