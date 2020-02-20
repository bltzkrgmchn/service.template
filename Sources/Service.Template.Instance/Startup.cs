using MassTransit;
using Microsoft.AspNetCore.Builder;
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
        public Startup(IHostingEnvironment environment)
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

            application.UseMvc();
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
            });

            // Регистрация шины.
            // Подробнее про регистрацию шины можно почитать здесь: https://masstransit-project.com/usage/
            services.AddSingleton(serviceProvider => Bus.Factory.CreateUsingRabbitMq(configure =>
            {
                BusConfiguration busConfiguration = this.configuration.GetSection("Bus").Get<BusConfiguration>();

                // Конфигурация подключения к шине, включающая в себя указание адреса и учетных данных.
                configure.Host(new Uri(busConfiguration.ConnectionString), host =>
                {
                    host.Username(busConfiguration.Username);
                    host.Password(busConfiguration.Password);

                    // Подтверждение получения гарантирует доставку сообщений за счет ухудшения производительности.
                    host.PublisherConfirmation = busConfiguration.PublisherConfirmation;
                });

                // Добавление Serilog для журналирования внутренностей MassTransit.
                configure.UseSerilog();

                // Регистрация очередей и их связи с потребителями сообщений.
                // В качестве метки сообщения используется полное имя класса сообщения, которое потребляет потребитель.
                configure.ReceiveEndpoint(typeof(GetAllPlaceholdersCommand).FullName, endpoint =>
                {
                    endpoint.Consumer<GetAllPlaceholdersConsumer>(serviceProvider);
                    EndpointConvention.Map<GetAllPlaceholdersCommand>(endpoint.InputAddress);
                });

                configure.ReceiveEndpoint(typeof(GetPlaceholderCommand).FullName, endpoint =>
                {
                    endpoint.Consumer<GetPlaceholderConsumer>(serviceProvider);
                    EndpointConvention.Map<GetPlaceholderCommand>(endpoint.InputAddress);
                });

                configure.ReceiveEndpoint(typeof(HealthcheckCommand).FullName, endpoint =>
                {
                    endpoint.Consumer<HealthcheckConsumer>(serviceProvider);
                    EndpointConvention.Map<HealthcheckCommand>(endpoint.InputAddress);
                });
            }));

            // Регистрация сервисов MassTransit.
            services.AddSingleton<IPublishEndpoint>(serviceProvider => serviceProvider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(serviceProvider => serviceProvider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(serviceProvider => serviceProvider.GetRequiredService<IBusControl>());

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

            // Регистрация сервиса шины MassTransit.
            services.AddSingleton<IHostedService, BusService>();
            Log.Information("Регистрация шины успешно завершена.");

            Log.Information("Начинается регистрация фильтра авторизации.");

            // Регистрация фильтра авторизации.
            services.AddScoped<AuthorizationFilter>();
            Log.Information("Регистрация фильтра авторизации успешно завершена.");

            Log.Information("Начинается регистрация сервисов MVC.");
            services.AddMvc();
            Log.Information("Регистрация сервисов MVC успешно завершена.");
        }
    }
}