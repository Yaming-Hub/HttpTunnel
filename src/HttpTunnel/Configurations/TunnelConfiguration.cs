using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpTunnel.Configurations
{
    public class TunnelConfiguration
    {
        public int TunnelPort { get; set; }

        public PortAndPath[] Apps { get; set; }

        public UrlReplaceRule[] UrlReplaceRules { get; set; }
    }
}
