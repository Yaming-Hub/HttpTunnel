using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Models;

namespace HttpTunnel.Contracts
{
    public interface IBackwardRequestHandler
    {
        Task<ResponseData> Handle(RequestData requestData);

        void SetResponse(int requestId, ResponseData responseData);
    }
}
