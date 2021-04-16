using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpTunnel.Configurations;
using HttpTunnel.Contracts;
using HttpTunnel.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HttpTunnel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            var mode = configuration.GetExecutionMode();

            if (mode == ExecutionMode.Client)
            {
                RunClient(configuration);
            }
            else
            {
                RunServer(configuration);
            }
        }

        private static void RunClient(IConfiguration configuration)
        {
            var clientConfig = configuration.GetClientConfiguration();
            var urls = clientConfig.Apps.Select(x => x.LocalAddress).ToArray();

            var host = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(cb => cb.AddConfiguration(configuration))
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseHttpSys()
                        .UseUrls(urls)
                        .UseStartup<ClientStartup>();
                    }).Build();

            // Start the connection client.
            var connectionClient = (IConnectionClient)host.Services.GetService(typeof(IConnectionClient));
            connectionClient.Start();

            host.Run();
        }

        private static void RunServer(IConfiguration configuration)
        {
            var serverConfig = configuration.GetServerConfiguration();
            var urls = new string[] { $"https://+:{serverConfig.TunnelPort}/tunnel" }
                .Concat(serverConfig.Apps.Select(x => x.LocalAddress))
                .ToArray();

            var host = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(cb => cb.AddConfiguration(configuration))
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseHttpSys()
                        .UseUrls(urls)
                        .UseStartup<ServerStartup>();
                    }).Build();

            host.Run();
        }

    }
}
