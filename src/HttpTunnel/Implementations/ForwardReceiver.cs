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
        private readonly IRequestClient requestClient;

        public ForwardReceiver(IRequestClient requestClient)
        {
            this.requestClient = requestClient;
        }

        public Task<ResponseData> Receive(RequestData requestData)
        {
            return this.requestClient.PostRequest(requestData);
        }
    }
}
