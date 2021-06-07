using System;
using Grpc.Core;
using Cachy.Events;
using System.Text;
using Google.Protobuf;

namespace Cachy.CommunicationIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            // Pinger.Run();
            ItemAdder.Run();
        }
    }


    public class Pinger
    {
        public static void Run()
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

    public class ItemAdder
    {
        public static void Run()
        {
            System.Console.WriteLine("yello");
            Channel channel = new Channel("127.0.0.1:5001", ChannelCredentials.Insecure);

            var client = new InsertItem.InsertItemClient(channel);
            var pong = client.InsertItem(new Item
            {
                Name = "Dummy",
                Ttl = new TimeToLive { Seconds = 100 },
                Data = ByteString.CopyFrom(Encoding.ASCII.GetBytes("hello"))
            });
            System.Console.WriteLine($"pong:{pong}");
            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }
    }
}
