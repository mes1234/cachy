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
                    // containers for handlers
                    services.AddSingleton<ConcurrentBag<IHandler<ItemEntinty>>>(new ConcurrentBag<IHandler<ItemEntinty>>());
                    services.AddSingleton<ConcurrentBag<IHandler<RequestForItem>>>(new ConcurrentBag<IHandler<RequestForItem>>());
                    // central message bus for system 
                    services.AddSingleton<ConcurrentQueue<IEntitie>>(new ConcurrentQueue<IEntitie>());
                    services.AddHostedService<Reciever>();
                    services.AddHostedService<Orchestrator>();
                    services.AddHostedService<LongTermStorage>();
                    services.AddHostedService<SnapshotStorage>();
                });
    }
}
