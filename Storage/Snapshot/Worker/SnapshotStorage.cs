using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using Cachy.Common.Maybe;
using Cachy.Common;
using Cachy.Storage.EventSource;

namespace Cachy.Storage.Snapshot.Worker
{
    public class SnapshotStorage : BackgroundService, IHandler
    {
        private readonly Snapshot<ItemEntinty> _snapshot = new();
        private readonly MaybeFactory _maybeFactory;
        public SnapshotStorage(
            ConcurrentBag<IHandler> handlers,
            MaybeFactory maybeFactory)
        {
            handlers.Add(this);
            _maybeFactory = maybeFactory;
        }

        private Task handle(ItemEntinty item)
        {

            SnapshotStorageItemEntinty itemValidated = _maybeFactory.GetMaybe<ItemEntinty, SnapshotStorageItemEntinty>(item);
            if (itemValidated == null) return Task.CompletedTask;
            var storedItem = new StoredItemEntity
            {
                Name = itemValidated.Name,
                Data = itemValidated.Data,
                Defined = itemValidated.Defined,
                Timestamp = itemValidated.Timestamp,
                TTL = itemValidated.TTL
            };
            _snapshot.Add(storedItem);
            return Task.CompletedTask;
        }

        private Task handle(RequestForItem item)
        {
            SnapshotRequestForItem itemValidated = _maybeFactory.GetMaybe<RequestForItem, SnapshotRequestForItem>(item);
            if (itemValidated == null) return Task.CompletedTask;
            lock (item)
            {
                if (item.Result != null) return Task.CompletedTask;
                item.Result = _snapshot.Get(itemValidated.Name);
                return Task.CompletedTask;
            }
        }

        private Task handle(ItemToRemoveEntity item)
        {
            ValidatedItemToRemoveEntity itemValidated = _maybeFactory.GetMaybe<ItemToRemoveEntity, ValidatedItemToRemoveEntity>(item);
            _snapshot.Remove(itemValidated.Name);
            return Task.CompletedTask;
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested != true)
            {
                try
                {

                    _snapshot.CheckTtl();
                    await Task.Delay(100);

                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex);
                }
            }

        }
    }
}
