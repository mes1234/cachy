using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Cachy.Common;
using Cachy.Storage.EventSource;

namespace Cachy.Storage
{
    public class SnapshotStorage : BackgroundService, IHandler
    {
        private readonly Snapshot<ItemEntinty> _snapshot = new();
        public SnapshotStorage(
            ConcurrentBag<IHandler> handlers)
        {
            handlers.Add(this);
        }

        private Task handle(ItemEntinty item)
        {
            _snapshot.Add(item);
            return Task.CompletedTask;
        }

        private Task handle(RequestForItem item)
        {
            item.Result = _snapshot.Get(item.Name);
            return Task.CompletedTask;
        }

        public async Task Handle(IEntitie item)
        {
            switch (item)
            {
                case ItemEntinty itemEntinty:
                    await handle(itemEntinty);
                    break;
                case RequestForItem requestForItem:
                    await handle(requestForItem);
                    break;
                default:
                    throw new NotSupportedException("Not supported item in queue");
            }
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
