using HttpTunnel.Contracts;
using HttpTunnel.Implementations;
using HttpTunnel.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace HttpTunnel.Hosting
{
    public class BackwardStartup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Singletons.BackwardRequestQueue);
            services.AddSingleton(Singletons.BackwardRequestHandler);
            services.AddSingleton<IBackwardReceiver, BackwardReceiver>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseMiddleware<LogRequestMiddleware>("backward");
            app.UseMiddleware<StatusMiddleware>("backward");
            app.UseMiddleware<BackwardReceiverMiddleware>();
        }
    }
}
