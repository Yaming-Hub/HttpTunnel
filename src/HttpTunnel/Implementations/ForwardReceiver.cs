using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Models;

namespace HttpTunnel.Implementations
{
    public class ForwardReceiver : IForwardReceiver
    {
        private readonly ITunnelClient tunnelClient;

        public ForwardReceiver(ITunnelClient tunnelClient)
        {
            this.tunnelClient = tunnelClient;
        }

        public Task<ResponseData> Receive(RequestData requestData)
        {
            return this.tunnelClient.PostRequest(requestData);
        }
    }
}
