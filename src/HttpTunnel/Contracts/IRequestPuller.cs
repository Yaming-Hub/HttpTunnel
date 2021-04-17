using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HttpTunnel.Contracts
{
    public interface IRequestPuller
    {
        Task Start(CancellationToken cancellationToken);
    }
}
