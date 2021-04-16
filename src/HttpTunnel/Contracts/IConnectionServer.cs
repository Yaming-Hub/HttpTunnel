using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HttpTunnel.Models;

namespace HttpTunnel.Contracts
{
    public interface IConnectionServer
    {
        Task Run(StreamWriter writer, CancellationToken cancellationToken);

        void SendRequest(RequestData request);
    }
}
