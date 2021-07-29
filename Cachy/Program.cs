using Cachy.Communication;
using Cachy.Dispatcher;
using Cachy.Storage;
using Cachy.Common;
using Cachy.Storage.Snapshot.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                    services.AddHostedService<Reciever>();
                    services.AddHostedService<LongTermStorage>();
                    services.AddHostedService<SnapshotStorage>();
                    services.AddHostedService<Orchestrator>();

                    CommonServicesRegistration.Register(services);
                    StorageServicesRegistration.Register(services);
                });
    }
}