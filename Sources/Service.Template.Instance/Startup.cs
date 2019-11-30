using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Service.Template.Services;
using Service.Template.Core;
using Service.Template.Data;

namespace Service.Template.Instance
{

    public class Startup
    {
        public void Configure(IApplicationBuilder application)
        {
            application.UseMvc();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<IPlaceholderService, PlaceholderService>();
            services.AddScoped<IPlaceholderGateway, PlaceholderGateway>();
            services.AddScoped<GetAllPlaceholdersConsumer>();
            services.AddScoped<GetSinglePlaceholderConsumer>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetAllPlaceholdersConsumer>();
                x.AddConsumer<GetSinglePlaceholderConsumer>();
            });

            services.AddSingleton(serviceProvider => Bus.Factory.CreateUsingInMemory(configure =>
            {
                configure.ReceiveEndpoint("placeholders.getall", endpoint =>
                {
                    endpoint.Consumer<GetAllPlaceholdersConsumer>(serviceProvider);
                    EndpointConvention.Map<GetAllPlaceholdersCommand>(endpoint.InputAddress);
                });

                configure.ReceiveEndpoint("placeholder.getsingle", endpoint =>
                {
                    endpoint.Consumer<GetSinglePlaceholderConsumer>(serviceProvider);
                    EndpointConvention.Map<GetSinglePlaceholderCommand>(endpoint.InputAddress);
                });
            }));

            services.AddSingleton<IPublishEndpoint>(serviceProvider => serviceProvider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(serviceProvider => serviceProvider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(serviceProvider => serviceProvider.GetRequiredService<IBusControl>());

            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IBus>().CreateRequestClient<GetAllPlaceholdersCommand>());
            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IBus>().CreateRequestClient<GetSinglePlaceholderCommand>());

            services.AddSingleton<IHostedService, BusService>();
        }
    }
}