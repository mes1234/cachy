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

        private readonly ConcurrentBag<IHandler<ItemEntinty>> _handlers;
        private readonly ConcurrentQueue<ItemEntinty> _queue;
        public Orchestrator(ConcurrentBag<IHandler<ItemEntinty>> handlers, ConcurrentQueue<ItemEntinty> Queue)
        {
            _handlers = handlers;
            _queue = Queue;
        }

        public async Task ScheduleAdding(ItemEntinty item)
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
                ItemEntinty item;
                if (_queue.TryDequeue(out item))
                {
                    await ScheduleAdding(item);
                }
                else
                {
                    await Task.Delay(10);
                }
            }
        }
    }
}
