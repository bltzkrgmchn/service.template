using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Template.Instance
{
    /// <inheritdoc/>
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

        /// <inheritdoc/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return this.busControl.StartAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.busControl.StopAsync(cancellationToken);
        }
    }
}