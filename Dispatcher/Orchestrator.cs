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
        private readonly ConcurrentQueue<IEntitie> _queue;
        public Orchestrator(
            ConcurrentBag<IHandler> Handlers,
            ConcurrentQueue<IEntitie> Queue)
        {
            _handlers = Handlers;
            _queue = Queue;
        }
        public async Task Schedule<T>(T item)
        where T : IEntitie
        {
            foreach (var handler in _handlers)
            {
                await handler.Handle(item);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested != true)
            {
                IEntitie item;
                if (_queue.TryDequeue(out item))
                {
                    await Schedule<IEntitie>(item);
                }
                else
                {
                    await Task.Delay(10);
                }
            }
        }
    }
}
