using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Configurations;

namespace HttpTunnel.Models
{
    public class TunnelStatus
    {
        public ExecutionMode Mode { get; set; }

        public TunnelConfiguration Configuration { get; set; }
    }
}
