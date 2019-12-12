using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Template.Consumers;
using Service.Template.Core;
using Service.Template.Data;

namespace Service.Template.Instance
{
    /// <summary>
    /// Инициализация приложения.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Конфигурация приложения.
        /// </summary>
        /// <param name="application">Приложение.</param>
        public void Configure(IApplicationBuilder application)
        {
            application.UseMvc();
        }

        /// <summary>
        /// Конфигурация сервисов.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddScoped<IPlaceholderService, PlaceholderService>();
            services.AddScoped<IPlaceholderRepository, PlaceholderRepository>();
            services.AddScoped<GetAllPlaceholdersConsumer>();
            services.AddScoped<GetPlaceholderConsumer>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetAllPlaceholdersConsumer>();
                x.AddConsumer<GetPlaceholderConsumer>();
            });

            services.AddSingleton(serviceProvider => Bus.Factory.CreateUsingInMemory(configure =>
            {
                configure.ReceiveEndpoint("command.placeholders.getall", endpoint =>
                {
                    endpoint.Consumer<GetAllPlaceholdersConsumer>(serviceProvider);
                    EndpointConvention.Map<GetAllPlaceholdersCommand>(endpoint.InputAddress);
                });

                configure.ReceiveEndpoint("command.placeholder.get", endpoint =>
                {
                    endpoint.Consumer<GetPlaceholderConsumer>(serviceProvider);
                    EndpointConvention.Map<GetPlaceholderCommand>(endpoint.InputAddress);
                });
            }));

            services.AddSingleton<IPublishEndpoint>(serviceProvider => serviceProvider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(serviceProvider => serviceProvider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(serviceProvider => serviceProvider.GetRequiredService<IBusControl>());

            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IBus>().CreateRequestClient<GetAllPlaceholdersCommand>());
            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IBus>().CreateRequestClient<GetPlaceholderCommand>());

            services.AddSingleton<IHostedService, BusService>();
        }
    }
}