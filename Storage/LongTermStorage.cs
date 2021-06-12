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
    public class LongTermStorage : BackgroundService, IHandler<ItemEntinty>, IHandler<RequestForItem>
    {

        private readonly Repository<ItemEntinty> _repository = new();
        public LongTermStorage(ConcurrentBag<IHandler<ItemEntinty>> handlers)
        {
            handlers.Add(this);
        }

        public Task Handle(ItemEntinty item)
        {
            _repository.Add(item);
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
