using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpTunnel.Configurations
{
    public class PortAndPath
    {
        public int Port { get; set; }

        public string Path { get; set; }

        public string LocalAddress => string.IsNullOrEmpty(this.Path)
            ? $"https://+:{this.Port}"
            : $"https://+:{this.Port}{this.Path}";
    }
}
