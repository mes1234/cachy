using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Cachy.Events;
using Cachy.Communication.Services;
using System.Collections.Concurrent;
using Cachy.Common;

namespace Cachy.Communication
{
    // Reciever is a class to handle external world communication
    public class Reciever : BackgroundService
    {
        private readonly int _port;
        private readonly ConcurrentQueue<ItemEntinty> _queue;
        private readonly string _host;

        public Reciever(ConcurrentQueue<ItemEntinty> Queue, int Port = 5001, string Host = "localhost")
        {
            _port = Port;
            _host = Host;
            _queue = Queue;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Server server = new Server
            {
                Services = {
                    PingPong.BindService(new PingPongService()),
                    InsertItem.BindService(new  InsertItemService(_queue))
                 },
                Ports = { new ServerPort(_host, _port, ServerCredentials.Insecure) }
            };
            await Task.Run(() => server.Start());
            await Task.Delay(100000, stoppingToken);
        }
    }
}
