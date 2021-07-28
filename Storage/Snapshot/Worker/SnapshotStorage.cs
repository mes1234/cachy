using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Cachy.Common.Maybe;
using Cachy.Common;
using Cachy.Storage.Persistance;

namespace Cachy.Storage.Snapshot.Worker
{
    public class SnapshotStorage : BackgroundService, IHandler
    {
        private readonly Snapshot<ItemEntinty> _snapshot = new Snapshot<ItemEntinty>();

        private readonly MaybeFactory _maybeFactory;

        public SnapshotStorage(
            ConcurrentBag<IHandler> handlers,
            MaybeFactory maybeFactory)
        {
            handlers.Add(this);
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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested != true)
            {
                try
                {
                    _snapshot.CheckTtl();
                    await Task.Delay(10);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex);
                }
            }
        }

        private Task Handle(ItemEntinty item)
        {
            SnapshotStorageItemEntinty itemValidated = _maybeFactory.GetMaybe<ItemEntinty, SnapshotStorageItemEntinty>(item);
            if (itemValidated == null)
                return Task.CompletedTask;
            var storedItem = new StoredItemEntity
            {
                Name = itemValidated.Name,
                Data = itemValidated.Data,
                Defined = itemValidated.Defined,
                Timestamp = itemValidated.Timestamp,
                TTL = itemValidated.TTL,
            };
            _snapshot.Add(storedItem);
            return Task.CompletedTask;
        }

        private Task Handle(RequestForItem item)
        {
            SnapshotRequestForItem itemValidated = _maybeFactory.GetMaybe<RequestForItem, SnapshotRequestForItem>(item);
            if (itemValidated == null)
                return Task.CompletedTask;
            lock (item)
            {
                if (item.Result != null)
                    return Task.CompletedTask;
                item.Result = _snapshot.Get(itemValidated.Name);
                return Task.CompletedTask;
            }
        }

        private Task Handle(ItemToRemoveEntity item)
        {
            ValidatedItemToRemoveEntity itemValidated = _maybeFactory.GetMaybe<ItemToRemoveEntity, ValidatedItemToRemoveEntity>(item);
            _snapshot.Remove(itemValidated.Name);
            return Task.CompletedTask;
        }
    }
}
