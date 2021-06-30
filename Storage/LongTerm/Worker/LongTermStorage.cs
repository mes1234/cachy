using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using Cachy.Common.Maybe;
using Cachy.Common;
using Cachy.Storage.EventSource;

namespace Cachy.Storage
{
    public class LongTermStorage : BackgroundService, IHandler
    {

        private readonly Repository<StoredItemEntity> _repository;
        private readonly MaybeFactory _maybeFactory;

        public LongTermStorage(
            ConcurrentBag<IHandler> handlers,
             Repository<StoredItemEntity> repository,
             MaybeFactory maybeFactory
             )
        {
            handlers.Add(this);
            _repository = repository;
            _maybeFactory = maybeFactory;
        }

        public Task handle(ItemEntinty item)
        {
            LongTermStorageItemEntinty itemValidated = _maybeFactory.GetMaybe<ItemEntinty, LongTermStorageItemEntinty>(item);

            if (itemValidated == null) return Task.CompletedTask;

            var storedItem = new StoredItemEntity
            {
                Data = itemValidated.Data,
                Name = itemValidated.Name,
                Timestamp = itemValidated.Timestamp,
                TTL = itemValidated.TTL,
                Defined = itemValidated.Defined
            };
            if (storedItem.Data.Length == 0)
                _repository.Remove(storedItem.Name);
            else
                _repository.Add(storedItem);
            return Task.CompletedTask;
        }

        private Task handle(RequestForItem item)
        {
            LongTermStorageRequestForItem itemValidated = _maybeFactory.GetMaybe<RequestForItem, LongTermStorageRequestForItem>(item);

            if (itemValidated == null) return Task.CompletedTask;

            lock (item)
            {
                if (item.Result != null) return Task.CompletedTask;

                item.Result = _repository.Get(itemValidated.Name, itemValidated.Revision);

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
