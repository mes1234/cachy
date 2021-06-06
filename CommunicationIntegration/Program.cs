using System;
using Grpc.Core;
using Communication;

namespace CommunicationIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("yello");
            Channel channel = new Channel("127.0.0.1:5001", ChannelCredentials.Insecure);

            var client = new PingPong.PingPongClient(channel);
            var pong = client.PingPong(new Ping { Message = "Ping" });
            System.Console.WriteLine($"pong:{pong}");
            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }
    }
}
