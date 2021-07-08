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

        private readonly IRepository<StoredItemEntity> _repository;
        private readonly MaybeFactory _maybeFactory;

        public LongTermStorage(
            ConcurrentBag<IHandler> handlers,
             IRepository<StoredItemEntity> repository,
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
                Defined = itemValidated.Defined,
                Active = true
            };

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

        private Task handle(ItemToRemoveEntity item)
        {
            ValidatedItemToRemoveEntity itemValidated = _maybeFactory.GetMaybe<ItemToRemoveEntity, ValidatedItemToRemoveEntity>(item);
            _repository.Remove(itemValidated.Name);
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {


                while (stoppingToken.IsCancellationRequested != true)
                {
                    // TODO LOCK REPOSITORY WHEN CHECKING ITEMS !!!!!
                    var counter = 0;
                    foreach (var item in _repository)
                    {
                        counter++;

                        if ((DateTime.Now - item.Timestamp).Seconds > item.TTL &&
                        item.Active)
                        {
                            _repository.Remove(item.Name);
                        }
                        if (counter == 10)
                        {
                            counter = 0;
                            await Task.Delay(100);
                        }
                    }
                    await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
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
                case ItemToRemoveEntity itemToRemoveEntity:
                    await handle(itemToRemoveEntity);
                    break;
                default:
                    throw new NotSupportedException("Not supported item in queue");
            }
        }
    }
}
