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
    public class SnapshotStorage : BackgroundService, IHandler<ItemEntinty>, IHandler<RequestForItem>
    {
        private readonly Snapshot<ItemEntinty> _snapshot = new();
        public SnapshotStorage(ConcurrentBag<IHandler<ItemEntinty>> handlers)
        {
            handlers.Add(this);
        }

        public Task Handle(ItemEntinty item)
        {
            _snapshot.Add(item);
            return Task.CompletedTask;
        }

        public Task Handle(RequestForItem item)
        {
            throw new NotImplementedException();
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
