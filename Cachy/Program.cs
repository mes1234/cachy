using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Cachy.Communication;
using Cachy.Dispatcher;
using Cachy.Storage;
using System.Collections.Concurrent;
using Cachy.Common;
using Cachy.Storage.EventSource;
using System.Collections;

namespace Cachy
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<Repository<StoredItemEntity>>();
                    services.AddTransient(typeof(IEvents<>), typeof(Events<>));
                    services.AddTransient(typeof(Dictionary<,>));
                    // containers for handlers
                    services.AddSingleton<ConcurrentBag<IHandler>>(new ConcurrentBag<IHandler>());
                    // central message bus for system 
                    services.AddSingleton<ConcurrentQueue<IEntity>>(new ConcurrentQueue<IEntity>());
                    services.AddHostedService<Reciever>();
                    services.AddHostedService<Orchestrator>();
                    services.AddHostedService<LongTermStorage>();
                    services.AddHostedService<SnapshotStorage>();
                });
    }
}
