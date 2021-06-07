using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Collections;
using System.Collections.Generic;
using Cachy.Common;

namespace Cachy.Dispatcher
{
    public class Orchestrator : BackgroundService
    {

        private readonly ConcurrentBag<IHandler> _handlers;
        public Orchestrator(ConcurrentBag<IHandler> handlers)
        {
            _handlers = handlers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested != true)
            {
                await Task.Delay(100);
            }
        }
    }
}
