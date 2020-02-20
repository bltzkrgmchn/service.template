using MassTransit;
using Microsoft.Extensions.Logging;
using Service.Template.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Template.Consumers
{
    /// <summary>
    /// Обработчик сообщения получения списка Placeholder.
    /// </summary>
    public class GetAllPlaceholdersConsumer : IConsumer<GetAllPlaceholdersCommand>
    {
        private readonly IPlaceholderService service;
        private readonly ILogger<GetAllPlaceholdersConsumer> logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GetAllPlaceholdersConsumer"/>.
        /// </summary>
        /// <param name="service">Сервисный объект для управления Placeholder.</param>
        /// <param name="logger">Абстракция над сервисом журналирования.</param>
        public GetAllPlaceholdersConsumer(IPlaceholderService service, ILogger<GetAllPlaceholdersConsumer> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Обработать сообщение.
        /// </summary>
        /// <param name="context">Контекст обработки сообщения.</param>
        /// <returns>Асинхронная операция <see cref="Task"/>.</returns>
        public async Task Consume(ConsumeContext<GetAllPlaceholdersCommand> context)
        {
            this.logger.LogInformation("Выполняется обработка сообщения получения всех Placeholder.");

            List<Placeholder> placeholders = this.service.Get();

            await context.RespondAsync(new GetAllPlaceholdersResponse { Placeholders = placeholders, Result = "success" });
        }
    }
}