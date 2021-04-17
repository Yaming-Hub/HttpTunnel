using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Implementations;

namespace HttpTunnel.Hosting
{
    /// <summary>
    /// This is a workaround to share singleton services cross different hosts.
    /// </summary>
    public static class Singletons
    {
        public static readonly ITunnelConnectionServer TunnelConnectionServer = new TunnelConnectionServer();
        public static readonly IBackwardRequestHandler BackwardRequestHandler = new BackwardRequestHandler(TunnelConnectionServer);
    }
}
