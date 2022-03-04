using System;
using System.Linq;
using HttpTunnel.Contracts;
using HttpTunnel.Models;
using Microsoft.Extensions.Logging;

namespace HttpTunnel.Implementations
{
    public sealed class LoopBreaker : ILoopBreaker
    {
        /// <summary>
        /// The header name.
        /// </summary>
        internal const string LoopHeaderName = "X-HttpTunnel-IsProxied";

        private readonly ILogger<LoopBreaker> logger;

        public LoopBreaker(ILogger<LoopBreaker> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public void ValidateAndTag(RequestData requestData)
        {
            if (requestData.Headers.Any(h => h.Name == LoopHeaderName))
            {
                this.logger.LogError("Loop detected");

                // The request already contains loop header, a loop is detected
                throw new InvalidOperationException("Loop detected");
            }

            requestData.Headers.Add(new HeaderData { Name = LoopHeaderName, Value = "true" });
        }
    }
}
