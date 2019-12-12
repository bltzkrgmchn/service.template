﻿using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Template.Instance
{
    public class BusService : IHostedService
    {
        private readonly IBusControl busControl;

        public BusService(IBusControl busControl)
        {
            this.busControl = busControl;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return this.busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.busControl.StopAsync(cancellationToken);
        }
    }
}