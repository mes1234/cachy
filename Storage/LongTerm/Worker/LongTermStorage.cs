using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Cachy.Common.Maybe;
using Cachy.Common;
using Cachy.Storage.Persistance;

namespace Cachy.Storage
{
    public class LongTermStorage : BackgroundService, IHandler
    {
        private readonly IRepository<StoredItemEntity> _repository;
        private readonly MaybeFactory _maybeFactory;

        public LongTermStorage(
             ConcurrentBag<IHandler> handlers,
             IRepository<StoredItemEntity> repository,
             MaybeFactory maybeFactory)
        {
            handlers.Add(this);
            _repository = repository;
            _maybeFactory = maybeFactory;
        }

        public async Task Handle(IEntity item)
        {
            switch (item)
            {
                case ItemEntinty itemEntinty:
                    await Handle(itemEntinty);
                    break;
                case RequestForItem requestForItem:
                    await Handle(requestForItem);
                    break;
                case ItemToRemoveEntity itemToRemoveEntity:
                    await Handle(itemToRemoveEntity);
                    break;
                default:
                    throw new NotSupportedException("Not supported item in queue");
            }
        }

        public Task Handle(ItemEntinty item)
        {
            LongTermStorageItemEntinty itemValidated = _maybeFactory.GetMaybe<ItemEntinty, LongTermStorageItemEntinty>(item);

            if (itemValidated == null)
                return Task.CompletedTask;

            var storedItem = new StoredItemEntity
            {
                Data = itemValidated.Data,
                Name = itemValidated.Name,
                Timestamp = itemValidated.Timestamp,
                TTL = itemValidated.TTL,
                Defined = itemValidated.Defined,
                Active = true,
            };

            _repository.Add(storedItem);
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested != true)
            {
                try
                {
                    _repository.CheckTtl();
                    await Task.Delay(10);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex);
                }
            }
        }

        private Task Handle(RequestForItem item)
        {
            LongTermStorageRequestForItem itemValidated = _maybeFactory.GetMaybe<RequestForItem, LongTermStorageRequestForItem>(item);

            if (itemValidated == null)
                return Task.CompletedTask;

            lock (item)
            {
                if (item.Result != null)
                    return Task.CompletedTask;

                item.Result = _repository.Get(itemValidated.Name, itemValidated.Revision);

                return Task.CompletedTask;
            }
        }

        private Task Handle(ItemToRemoveEntity item)
        {
            ValidatedItemToRemoveEntity itemValidated = _maybeFactory.GetMaybe<ItemToRemoveEntity, ValidatedItemToRemoveEntity>(item);
            _repository.Remove(itemValidated.Name);
            return Task.CompletedTask;
        }
    }
}
