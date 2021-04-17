using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

            var modes = configuration.GetExecutionMode();
            List<Task> tasks = new List<Task>();
            foreach (var mode in modes)
            {
                switch (mode)
                {
                    case ExecutionMode.Backward:
                        tasks.Add(RunBackwardAsync(configuration));
                        break;

                    case ExecutionMode.Forward:
                        tasks.Add(RunForwardAsync(configuration));
                        break;
                }
            }

            Task.WaitAll(tasks.ToArray());
        }

        private static Task RunForwardAsync(IConfiguration configuration)
        {
            var clientConfig = configuration.GetForwardConfiguration();
            var urls = clientConfig.Apps.Select(x => x.LocalAddress).ToArray();

            var forwardHost = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(cb => cb.AddConfiguration(configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseHttpSys()
                    .UseUrls(urls)
                    .UseStartup<ForwardStartup>();
                })
                .Build();

            // Start the connection client.
            var cancellationTokenSource = new CancellationTokenSource();
            var connectionClient = (IRequestPuller)forwardHost.Services.GetService(typeof(IRequestPuller));
            connectionClient.Start(cancellationTokenSource.Token);

            return forwardHost.RunAsync().ContinueWith(t => cancellationTokenSource.Cancel());
        }

        private static Task RunBackwardAsync(IConfiguration configuration)
        {
            var serverConfig = configuration.GetBackwardConfiguration();

            var tunnelUrl = $"https://+:{serverConfig.TunnelPort}/tunnel";
            var tunnelHost = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(cb => cb.AddConfiguration(configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseHttpSys()
                    .UseUrls(tunnelUrl)
                    .UseStartup<TunnelStartup>();
                })
                .Build();

            var backwardUrls = serverConfig.Apps.Select(x => x.LocalAddress).ToArray();
            var backwardHost = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(cb => cb.AddConfiguration(configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseHttpSys()
                    .UseUrls(backwardUrls)
                    .UseStartup<BackwardStartup>();
                })
                .Build();

            var tunnelTask = tunnelHost.RunAsync();
            var backwardTask = backwardHost.RunAsync();

            return Task.WhenAll(tunnelTask, backwardTask);
        }

    }
}
