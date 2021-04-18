using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HttpTunnel.Configurations
{
    public class UrlReplaceRule
    {
        public string Name { get; set; }

        public string Pattern { get; set; }

        public string Replacement { get; set; }
    }
}
