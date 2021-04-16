using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Models;

namespace HttpTunnel.Implementations
{
    public class BackwardReceiver : IBackwardReceiver
    {
        private readonly IBackwardRequestHandler backwardRequestHandler;

        public BackwardReceiver(IBackwardRequestHandler backwardRequestHandler)
        {
            this.backwardRequestHandler = backwardRequestHandler;
        }

        public Task<ResponseData> Receive(RequestData requestData)
        {
            return this.backwardRequestHandler.Handle(requestData);
        }
    }
}
