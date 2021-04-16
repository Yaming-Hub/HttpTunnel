using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpTunnel.Configurations
{
    public class Redirect
    {
        public PortAndPath From { get; set; }

        public PortAndPath To { get; set; }
    }
}
