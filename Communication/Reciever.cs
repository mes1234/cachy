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
        private readonly ConcurrentQueue<ItemEntinty> _queueIn;
        private readonly ConcurrentQueue<RequestForItem> _queueOut;
        private readonly string _host;

        public Reciever(ConcurrentQueue<ItemEntinty> QueueIn, ConcurrentQueue<RequestForItem> QueueOut, int Port = 5001, string Host = "localhost")
        {
            _port = Port;
            _host = Host;
            _queueIn = QueueIn;
            _queueOut = QueueOut;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Server server = new Server
            {
                Services = {
                    PingPong.BindService(new PingPongService()),
                    InsertItem.BindService(new  InsertItemService(_queueIn)),
                    GetItem.BindService(new GetItemService(_queueOut))
                 },
                Ports = { new ServerPort(_host, _port, ServerCredentials.Insecure) }
            };
            await Task.Run(() => server.Start());
            await Task.Delay(100000, stoppingToken);
        }
    }
}
