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
                    // container for all possible handlers for new messages
                    services.AddSingleton<ConcurrentBag<IHandler<ItemEntinty>>>(new ConcurrentBag<IHandler<ItemEntinty>>());
                    // central message bus for system - new items added
                    services.AddSingleton<ConcurrentQueue<ItemEntinty>>(new ConcurrentQueue<ItemEntinty>());
                    // central message bus for system - new request for items
                    services.AddSingleton<ConcurrentQueue<RequestForItem>>(new ConcurrentQueue<RequestForItem>());
                    services.AddHostedService<Reciever>();
                    services.AddHostedService<Orchestrator>();
                    services.AddHostedService<LongTermStorage>();
                    services.AddHostedService<SnapshotStorage>();
                });
    }
}
