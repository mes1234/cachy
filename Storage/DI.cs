
using Cachy.Common;
using Cachy.Storage.EventSource;
using Microsoft.Extensions.DependencyInjection;

namespace Cachy.Storage
{
    public static class StorageServicesRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient(typeof(IEvents<>), typeof(Events<>));
            services.AddTransient<Repository<StoredItemEntity>>();
        }
    }
}