using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpTunnel.Contracts
{
    public interface IConnectionClient
    {
        Task Start();
    }
}
