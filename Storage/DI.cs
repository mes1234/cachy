
using Cachy.Common;
using Cachy.Storage.EventSource;
using Cachy.Storage.LongTerm.Validation.Inbound;
using Cachy.Storage.Snapshot.Validation.Inbound;
using Microsoft.Extensions.DependencyInjection;

namespace Cachy.Storage
{
    public static class StorageServicesRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<Repository<StoredItemEntity>>();

            services.AddTransient<IValidator<ItemEntinty, LongTermStorageItemEntinty>, LTSItemEntintyValidator>();
            services.AddTransient<IValidator<RequestForItem, LongTermStorageRequestForItem>, LTSRequestForItemValidator>();
            services.AddTransient<IValidator<RequestForItem, SnapshotRequestForItem>, SnapshotRequestForItemValidator>();

        }
    }
}