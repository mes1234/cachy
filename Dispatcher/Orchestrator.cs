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

        private readonly ConcurrentBag<IHandler<ItemEntinty>> _addHandlers;
        private readonly ConcurrentBag<IHandler<RequestForItem>> _getHandlers;
        private readonly ConcurrentQueue<IEntitie> _queue;
        public Orchestrator(
            ConcurrentBag<IHandler<ItemEntinty>> addHandlers,
            ConcurrentBag<IHandler<RequestForItem>> getHandlers,
             ConcurrentQueue<IEntitie> Queue)
        {
            _addHandlers = addHandlers;
            _getHandlers = getHandlers;
            _queue = Queue;
        }

        public async Task ScheduleAdding(ItemEntinty item)
        {
            foreach (var handler in _addHandlers)
            {
                await handler.Handle(item);
            }
        }
        public async Task ScheduleGetting(RequestForItem item)
        {
            foreach (var handler in _getHandlers)
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
                    switch (item)
                    {
                        case ItemEntinty itemEntinty:
                            await ScheduleAdding(itemEntinty);
                            break;
                        case RequestForItem requestForItem:
                            await ScheduleGetting(requestForItem);
                            break;
                        default:
                            throw new NotSupportedException("Not supported item in queue");
                    };

                }
                else
                {
                    await Task.Delay(10);
                }
            }
        }
    }
}
