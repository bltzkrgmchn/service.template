using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nancy.Owin;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Template.Instance
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(x => x.UseNancy());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<HelloWorldHostedService>();
        }

        public class HelloWorldHostedService : BackgroundService
        {
            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine("Hello World");
                    await Task.Delay(10000, stoppingToken);
                }
            }
        }
    }
}