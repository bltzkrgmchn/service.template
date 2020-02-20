using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Template.Instance
{
    /// <summary>
    /// Сервис шины MassTransit.
    /// Подробности использования интерфейса интерфейс <see cref="IHostedService"/> можно почитать здесь:
    /// http://docs.microsoft.com/ru-ru/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-2.2.
    /// </summary>
    public class BusService : IHostedService
    {
        private readonly IBusControl busControl;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="BusService"/>.
        /// </summary>
        /// <param name="busControl">Контролер шины.</param>
        public BusService(IBusControl busControl)
        {
            this.busControl = busControl;
        }

        /// <summary>
        /// Метод, выполняющийся во время запуска сервиса.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return this.busControl.StartAsync(cancellationToken);
        }

        /// <summary>
        /// Метод, выполняющийся во время остановки сервиса.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.busControl.StopAsync(cancellationToken);
        }
    }
}