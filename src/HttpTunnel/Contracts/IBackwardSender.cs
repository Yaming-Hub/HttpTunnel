using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Models;

namespace HttpTunnel.Contracts
{
    public interface IBackwardSender
    {
        /// <summary>
        /// Sends the request and return the reponse.
        /// </summary>
        /// <param name="requestData">The request.</param>
        void Send(RequestData requestData);
    }
}
