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
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            foreach (var config in configuration.AsEnumerable())
            {
                Console.WriteLine($"{config.Key}={config.Value}");
            }

            if (configuration.GetValue<bool?>("PauseOnStart").GetValueOrDefault())
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

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
            Console.WriteLine("Starting forward host...");

            var forwardConfig = configuration.GetForwardConfiguration();
            if (forwardConfig == null)
            {
                throw new ArgumentException("Forward configuration is missing.");
            }

            var urls = forwardConfig.Apps.Select(x => x.LocalAddress).ToArray();

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
            Console.WriteLine("Starting backward host...");
            var backwardConfig = configuration.GetBackwardConfiguration();
            if (backwardConfig == null)
            {
                throw new ArgumentException("Bacward configuration is missing.");
            }

            var tunnelUrl = $"https://+:{backwardConfig.TunnelPort}/tunnel";
            var tunnelHost = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(cb => cb.AddConfiguration(configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseHttpSys()
                    .UseUrls(tunnelUrl)
                    .UseStartup<TunnelStartup>();
                })
                .Build();

            var backwardUrls = backwardConfig.Apps.Select(x => x.LocalAddress).ToArray();
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
