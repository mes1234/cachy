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

        private readonly Repository<StoredItemEntity> _repository;
        public LongTermStorage(ConcurrentBag<IHandler> handlers, Repository<StoredItemEntity> Repository)
        {
            handlers.Add(this);
            _repository = Repository;
        }

        public Task handle(ItemEntinty item)
        {
            var storedItem = new StoredItemEntity
            {
                Data = item.Data,
                Name = item.Name,
                Timestamp = item.Timestamp,
                TTL = item.TTL
            };
            _repository.Add(storedItem);
            return Task.CompletedTask;
        }

        private Task handle(RequestForItem item)
        {
            lock (item)
            {
                if (item.Revision > 0)
                {
                    item.Result = _repository.Get(item.Name, item.Revision);
                }
                return Task.CompletedTask;
            }


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested != true)
            {
                await Task.Delay(100);
            }
        }

        public async Task Handle(IEntity item)
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
