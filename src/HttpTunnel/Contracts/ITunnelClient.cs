using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Models;

namespace HttpTunnel.Contracts
{
    public interface ITunnelClient
    {

        Task<Stream> Connect();

        Task<ResponseData> PostRequest(RequestData requestData);

        Task PutResponse(int requestId, ResponseData responseData);
    }
}
