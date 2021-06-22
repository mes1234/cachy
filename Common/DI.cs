using System.Collections.Concurrent;
using Cachy.Common.Converter;
using Cachy.Common.Maybe;
using Cachy.Common.Validator;
using Cachy.Common.Validator.Implementation;
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

            services.AddTransient<IConverter<RequestForItem, RequestForItemValidated>, SimpleRequestConverter>();
            services.AddTransient<IValidator<RequestForItem, RequestForItemValidated>, LongTermStorageItemRequestForItem>();

            services.AddTransient<IConverter<ItemEntinty, LongTermStorageItemEntinty>, SimpleConverter>();
            services.AddTransient<IValidator<ItemEntinty, LongTermStorageItemEntinty>, LongTermStorageItemEntintyValidator>();
        }
    }
}