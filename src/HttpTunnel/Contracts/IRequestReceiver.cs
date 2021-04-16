using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Models;

namespace HttpTunnel.Contracts
{
    public interface IRequestReceiver
    {
        /// <summary>
        /// Receive the request and return the reponse.
        /// </summary>
        /// <param name="requestData">The request.</param>
        /// <returns>The response.</returns>
        Task<ResponseData> Receive(RequestData requestData);
    }
}
