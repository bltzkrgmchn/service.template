using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Service.Template.Consumers.Healthchecks
{
    /// <summary>
    /// Обработчик сообщения получения состояния службы.
    /// </summary>
    public class HealthcheckConsumer : IConsumer<HealthcheckCommand>
    {
        private readonly ILogger<HealthcheckConsumer> logger;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="HealthcheckConsumer"/>.
        /// </summary>
        /// <param name="logger">Абстракция над сервисом журналирования.</param>
        public HealthcheckConsumer(ILogger<HealthcheckConsumer> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Обработать сообщение.
        /// </summary>
        /// <param name="context">Контекст обработки сообщения.</param>
        /// <returns>Асинхронная операция <see cref="Task"/>.</returns>
        public async Task Consume(ConsumeContext<HealthcheckCommand> context)
        {
            this.logger.LogInformation("Выполняется получение состояния службы.");

            await context.RespondAsync(new HealthcheckResponse { Result = "success" });
        }
    }
}