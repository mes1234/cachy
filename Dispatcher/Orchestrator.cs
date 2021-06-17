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
        private readonly ConcurrentQueue<IEntity> _queue;
        public Orchestrator(
            ConcurrentBag<IHandler> Handlers,
            ConcurrentQueue<IEntity> Queue)
        {
            _handlers = Handlers;
            _queue = Queue;
        }
        public async Task Schedule<T>(T item)
        where T : IEntity
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
                IEntity item;
                if (_queue.TryDequeue(out item))
                {
                    await Schedule(item);
                }
                else
                {
                    Thread.Sleep(0);
                }
            }
        }
    }
}
