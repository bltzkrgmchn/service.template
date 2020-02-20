using MassTransit;
using Microsoft.Extensions.Logging;
using Service.Template.Core;
using System.Threading.Tasks;

namespace Service.Template.Consumers
{
    /// <summary>
    /// Обработчик сообщения получения Placeholder.
    /// </summary>
    public class GetPlaceholderConsumer : IConsumer<GetPlaceholderCommand>
    {
        private readonly IPlaceholderService service;
        private readonly ILogger<GetAllPlaceholdersConsumer> logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GetPlaceholderConsumer"/>.
        /// </summary>
        /// <param name="service">Сервисный объект для управления Placeholder.</param>
        /// /// <param name="logger">Абстракция над сервисом журналирования.</param>
        public GetPlaceholderConsumer(IPlaceholderService service, ILogger<GetAllPlaceholdersConsumer> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Обработать сообщение.
        /// </summary>
        /// <param name="context">Контекст обработки сообщения.</param>
        /// <returns>Асинхронная операция <see cref="Task"/>.</returns>
        public async Task Consume(ConsumeContext<GetPlaceholderCommand> context)
        {
            this.logger.LogInformation($"Выполняется обработка сообщения получения Placeholder с идентификатором {context.Message.Id}.");

            Placeholder placeholder = this.service.Get(context.Message.Id);

            if (placeholder != null)
            {
                await context.RespondAsync(new GetPlaceholderResponse { Placeholder = placeholder, Result = "success" });
            }

            await context.RespondAsync(new GetPlaceholderResponse { Result = "not-found" });
        }
    }
}