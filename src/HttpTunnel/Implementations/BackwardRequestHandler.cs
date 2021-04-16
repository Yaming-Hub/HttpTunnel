using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Models;

namespace HttpTunnel.Implementations
{
    public class BackwardRequestHandler : IBackwardRequestHandler
    {
        private readonly IConnectionServer connectionServer;
        private readonly ConcurrentDictionary<int, BackwardRequestState> states;

        public BackwardRequestHandler(IConnectionServer connectionServer)
        {
            this.connectionServer = connectionServer;
            this.states = new ConcurrentDictionary<int, BackwardRequestState>();
        }

        public Task<ResponseData> Handle(RequestData requestRata)
        {
            var state = new BackwardRequestState
            {
                Request = requestRata,
                ResponseCompletionSource = new TaskCompletionSource<ResponseData>()
            };

            if (!this.states.TryAdd(requestRata.Id, state))
            {
                throw new InvalidOperationException($"Request {requestRata.Id} is already handled.");
            }

            this.connectionServer.SendRequest(requestRata);
            return state.ResponseCompletionSource.Task;
        }

        public void SetResponse(int requestId, ResponseData responseData)
        {
            if (!this.states.TryGetValue(requestId, out BackwardRequestState state))
            {
                throw new ArgumentException($"The request id {requestId} is not found.");
            }

            state.ResponseCompletionSource.SetResult(responseData);
        }

        private class BackwardRequestState
        {
            public RequestData Request { get; set; }

            public TaskCompletionSource<ResponseData> ResponseCompletionSource { get; set; }
        }
    }
}
