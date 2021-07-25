using System.Collections.Concurrent;
using Cachy.Common.Converter;
using Cachy.Common.Maybe;
using Microsoft.Extensions.DependencyInjection;

namespace Cachy.Common
{
    public static class CommonServicesRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<MaybeFactory>();
            services.AddTransient(typeof(Maybe<,>));

            // containers for handlers
            services.AddSingleton<ConcurrentBag<IHandler>>(new ConcurrentBag<IHandler>());

            // central message bus for system
            services.AddSingleton<ConcurrentQueue<IEntity>>(new ConcurrentQueue<IEntity>());

            services.AddTransient<IConverter<ItemEntinty, LongTermStorageItemEntinty>, LongTermStorageItemEntintyConverter>();
            services.AddTransient<IConverter<ItemEntinty, SnapshotStorageItemEntinty>, SnapshotItemEntintyConverter>();
            services.AddTransient<IConverter<RequestForItem, SnapshotRequestForItem>, SnapshotRequestForItemConverter>();
            services.AddTransient<IConverter<RequestForItem, LongTermStorageRequestForItem>, LongTermStorageRequestConverter>();
            services.AddTransient<IConverter<ItemToRemoveEntity, ValidatedItemToRemoveEntity>, SnapshotItemToRemoveConverted>();
        }
    }
}