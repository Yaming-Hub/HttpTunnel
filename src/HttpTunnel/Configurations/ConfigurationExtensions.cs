using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HttpTunnel.Configurations
{
    public static class ConfigurationExtensions
    {
        public static ExecutionMode GetExecutionMode(this IConfiguration configuration)
        {
            var modeParameter = configuration.GetValue<string>("mode");
            if (modeParameter == null || !Enum.TryParse<ExecutionMode>(modeParameter, out ExecutionMode mode))
            {
                throw new ArgumentException("Bad mode parameter", "mode");
            }

            return mode;
        }

        public static TunnelClientConfiguration GetClientConfiguration(this IConfiguration configuration)
            => configuration.GetSection("Client").Get<TunnelClientConfiguration>();

        public static TunnelServerConfiguration GetServerConfiguration(this IConfiguration configuration)
            => configuration.GetSection("Server").Get<TunnelServerConfiguration>();
    }
}
