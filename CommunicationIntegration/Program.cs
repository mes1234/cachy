using System;
using Grpc.Core;
using Cachy.Events;
using System.Text;
using Google.Protobuf;
using PowerArgs;

namespace Cachy.CommunicationIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            Config config = Args.Parse<Config>(args);
            var command = CommandFactory.GetCommand(config);
            command();
            System.Console.WriteLine("Press key to exit");
            Console.ReadKey();
        }
    }

    public class CommandFactory
    {
        public static Action GetCommand(Config config)
        {
            return config.Mode switch
            {
                "ping" => () => Pinger.Run(),
                "add" => () => ItemAdder.Run(),
                "get" => () => ItemGetter.Run(),
                "older" => () => ItemGetterOlder.Run(),
                _ => throw new NotSupportedException("This option is not supported")
            };
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

        }
    }
    public class ItemGetter
    {
        public static void Run()
        {
            System.Console.WriteLine("I will retrieve item : yello");
            Channel channel = new Channel("127.0.0.1:5001", ChannelCredentials.Insecure);

            var client = new GetItem.GetItemClient(channel);
            var res = client.Get(new ItemToRetrieve
            {
                Name = "yello"
            });
            System.Console.WriteLine($"res:{res}");
            channel.ShutdownAsync().Wait();

        }
    }
    public class ItemGetterOlder
    {
        public static void Run()
        {
            System.Console.WriteLine("I will retrieve item : yello");
            Channel channel = new Channel("127.0.0.1:5001", ChannelCredentials.Insecure);

            var client = new GetItem.GetItemClient(channel);
            var res = client.Get(new ItemToRetrieve
            {
                Name = "yello",
                Revision = 1
            });
            System.Console.WriteLine($"res:{res}");
            channel.ShutdownAsync().Wait();

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
                Name = "yello",
                Ttl = new TimeToLive { Seconds = 100 },
                Data = ByteString.CopyFrom(Encoding.ASCII.GetBytes("hello"))
            });
            System.Console.WriteLine($"pong:{pong}");
            channel.ShutdownAsync().Wait();
        }
    }
}
