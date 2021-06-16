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
    public class LongTermStorage : BackgroundService, IHandler
    {

        private readonly Repository<ItemEntinty> _repository = new();
        public LongTermStorage(ConcurrentBag<IHandler> handlers)
        {
            handlers.Add(this);
        }

        public Task handle(ItemEntinty item)
        {
            _repository.Add(item);
            return Task.CompletedTask;
        }

        private Task handle(RequestForItem item)
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
    }
}
