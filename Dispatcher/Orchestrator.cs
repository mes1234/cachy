using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Cachy.Common;

namespace Cachy.Dispatcher
{
    public class Orchestrator : BackgroundService
    {
        private readonly ConcurrentBag<IHandler> _handlers;

        private readonly ConcurrentQueue<IEntity> _queue;

        public Orchestrator(
            ConcurrentBag<IHandler> handlers,
            ConcurrentQueue<IEntity> queue)
        {
            _handlers = handlers;
            _queue = queue;
        }

        public async Task Schedule<T>(T item)
        where T : IEntity
        {
            foreach (var handler in _handlers)
            {
                try
                {
                    await handler.Handle(item);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex);
                }
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
                    await Task.Delay(10);
                }
            }
        }
    }
}
