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
        private readonly ConcurrentQueue<ItemEntinty> _queue;
        public Orchestrator(ConcurrentBag<IHandler> handlers, ConcurrentQueue<ItemEntinty> Queue)
        {
            _handlers = handlers;
            _queue = Queue;
        }

        public Task Schedule(ItemEntinty item)
        {
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested != true)
            {
                ItemEntinty item;
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
