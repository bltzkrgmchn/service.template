using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Service.Template.Consumers;
using Service.Template.Consumers.Healthchecks;
using Service.Template.Core;
using Service.Template.Data;
using Service.Template.WebApi;
using System;

namespace Service.Template.Instance
{
    /// <summary>
    /// Инициализация приложения.
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Startup"/>.
        /// </summary>
        public Startup(IHostEnvironment environment)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.production.json", true, true);

            this.configuration = builder.Build();
        }

        /// <summary>
        /// Конфигурация приложения.
        /// </summary>
        /// <param name="application">Приложение.</param>
        public void Configure(IApplicationBuilder application)
        {
            application.UseCors("AllowAll");

            application.UseRouting();
            application.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        /// <summary>
        /// Конфигурация сервисов.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(this.configuration)
                .CreateLogger();

            Log.Information("Начинается регистрация политик CORS.");

            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            Log.Information("Регистрация политик CORS успешно завершена.");

            Log.Information("Начинается регистрация сервисов.");

            // Все сервисы имеют stateless реализацию и могут быть зарегистрированы как singleton.
            services.AddSingleton<IPlaceholderService, PlaceholderService>();

            services.AddSingleton<IPlaceholderRepository, PlaceholderRepository>(o =>
                new PlaceholderRepository(
                    this.configuration.GetConnectionString("Placeholders"),
                    o.GetRequiredService<ILogger<PlaceholderRepository>>()));

            services.AddSingleton<GetAllPlaceholdersConsumer>();

            services.AddSingleton<GetPlaceholderConsumer>();

            Log.Information("Регистрация сервисов успешно завершена.");

            Log.Information("Начинается регистрация шины.");

            // Регистрация потребителей сообщений
            services.AddMassTransit(x =>
            {
                x.AddConsumer<GetAllPlaceholdersConsumer>();
                x.AddConsumer<GetPlaceholderConsumer>();
                x.AddConsumer<HealthcheckConsumer>();

                x.AddRequestClient<AuthorizeCommand>();

                x.UsingRabbitMq((context, configuration) =>
                {
                    BusConfiguration busConfiguration = this.configuration.GetSection("Bus").Get<BusConfiguration>();

                    configuration.Host(busConfiguration.ConnectionString, h =>
                    {
                        h.Username(busConfiguration.Username);
                        h.Password(busConfiguration.Password);

                        h.PublisherConfirmation = busConfiguration.PublisherConfirmation;
                    });

                    configuration.ConfigureEndpoints(context);
                });
            });

            // Регистрация сервисов MassTransit.
            services.AddMassTransitHostedService();

            // Регистрация клиентов для запроса данных от потребителей сообщений из api.
            // Каждый клиент зарегистрирован таким образом, что бы в рамках каждого запроса к api существовал свой клиент.
            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IBus>()
                .CreateRequestClient<GetAllPlaceholdersCommand>());
            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IBus>()
                .CreateRequestClient<GetPlaceholderCommand>());
            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IBus>()
                .CreateRequestClient<HealthcheckCommand>());
            services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IBus>()
                .CreateRequestClient<AuthorizeCommand>());

            Log.Information("Регистрация шины успешно завершена.");

            Log.Information("Начинается регистрация фильтра авторизации.");

            // Регистрация фильтра авторизации.
            services.AddScoped<AuthorizationFilter>();
            Log.Information("Регистрация фильтра авторизации успешно завершена.");

            Log.Information("Начинается регистрация сервисов MVC.");
            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(HealthController).Assembly));
            Log.Information("Регистрация сервисов MVC успешно завершена.");
        }
    }
}