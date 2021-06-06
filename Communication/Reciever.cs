using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Hosting;

namespace Communication
{
    public class Reciever : BackgroundService
    {
        private readonly int _port;
        private readonly string _host;
        public Reciever(int Port = 5001, string Host = "localhost")
        {
            _port = Port;
            _host = Host;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Server server = new Server
            {
                Services = { PingPong.BindService(new PingPongImpl()) },
                Ports = { new ServerPort(_host, _port, ServerCredentials.Insecure) }
            };
            await Task.Run(() => server.Start());
            await Task.Delay(100000, stoppingToken);
        }
    }
}
