using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BotServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string url = args[0];

            CreateHostBuilder(args, url).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string url) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
#pragma warning disable CA1416 // Validate platform compatibility
                    webBuilder.UseHttpSys()
#pragma warning restore CA1416 // Validate platform compatibility
                        .UseUrls(url)
                        .UseStartup<Startup>();
                });
    }
}
