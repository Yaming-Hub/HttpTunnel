using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Contracts;
using HttpTunnel.Implementations;
using HttpTunnel.Models;

namespace HttpTunnel.Hosting
{
    /// <summary>
    /// This is a workaround to share singleton services cross different hosts.
    /// </summary>
    public static class Singletons
    {
        public static readonly IAsyncQueue<RequestData> BackwardRequestQueue = new AsyncQueue<RequestData>();
        public static readonly IBackwardRequestHandler BackwardRequestHandler = new BackwardRequestHandler(BackwardRequestQueue);
    }
}
