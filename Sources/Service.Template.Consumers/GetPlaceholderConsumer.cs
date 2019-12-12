using MassTransit;
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

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GetPlaceholderConsumer"/>.
        /// </summary>
        /// <param name="service">Сервисный объект для управления Placeholder.</param>
        public GetPlaceholderConsumer(IPlaceholderService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Обработать сообщение.
        /// </summary>
        /// <param name="context">Контекст обработки сообщения.</param>
        /// <returns>Асинхронная операция <see cref="Task"/>.</returns>
        public async Task Consume(ConsumeContext<GetPlaceholderCommand> context)
        {
            Placeholder placeholder = this.service.Get(context.Message.Id);

            await context.RespondAsync(new GetPlaceholderResponse { Placeholder = placeholder });
        }
    }
}