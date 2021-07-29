using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Cachy.Events;
using Cachy.Communication.Services;
using Cachy.Common;

namespace Cachy.Communication
{
    // Reciever is a class to handle external world communication
    public class Reciever : BackgroundService
    {
        private readonly int _port;
        private readonly ConcurrentQueue<IEntity> _queue;
        private readonly string _host;

        public Reciever(ConcurrentQueue<IEntity> queue, int port = 5001, string host = "localhost")
        {
            _port = port;
            _host = host;
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Server server = new Server
            {
                Services =
                {
                    InsertItem.BindService(new InsertItemService(_queue)),
                    GetItem.BindService(new GetItemService(_queue)),
                    RemoveItem.BindService(new RemoveItemService(_queue)),
                },
                Ports = { new ServerPort(_host, _port, ServerCredentials.Insecure) },
            };
            await Task.Run(() => server.Start());
            while (true)
            {
                await Task.Delay(100000, stoppingToken);
            }
        }
    }
}
