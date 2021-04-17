using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace HttpTunnel.Configurations
{
    public static class ConfigurationExtensions
    {
        public static IEnumerable<ExecutionMode> GetExecutionMode(this IConfiguration configuration)
        {
            var modeParameter = configuration.GetValue<string>("mode");
            if (modeParameter == null)
            {
                throw new ArgumentException($"Mode parameter is not set.");
            }

            foreach (var modeString in modeParameter.Split(','))
            {
                if (!Enum.TryParse<ExecutionMode>(modeString.Trim(), true, out ExecutionMode mode))
                {
                    throw new ArgumentException($"Bad mode parameter value: {modeParameter}", "mode");
                }

                yield return mode;
            }
        }

        public static ForwardConfiguration GetForwardConfiguration(this IConfiguration configuration)
            => configuration.GetSection("Forward").Get<ForwardConfiguration>();

        public static BackwardConfiguration GetBackwardConfiguration(this IConfiguration configuration)
            => configuration.GetSection("Backward").Get<BackwardConfiguration>();
    }
}
