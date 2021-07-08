using System;
using Grpc.Core;
using Cachy.Events;
using System.Text;
using Google.Protobuf;
using PowerArgs;
using CachyClient;
using System.Threading.Tasks;
using System.Linq;

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
                "ping" => () => Pinger.Run(), // ping service
                "add" => async () => await ItemAdder.Run(), //add single item to cachy
                "get" => () => ItemGetter.Run(), //get last revision
                "getwrong" => () => ItemGetterNotFound.Run(), //get item which doesn't exist
                "older" => () => ItemGetterOlder.Run(), //get older revision
                "olderwrong" => () => ItemGetterOlder.Run(), //get older revison which doesn't exist
                "remove" => () => ItemRemove.Run(), //remove item
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
    public class ItemGetterNotFound
    {
        public static void Run()
        {
            System.Console.WriteLine("I will retrieve item : yello");
            Channel channel = new Channel("127.0.0.1:5001", ChannelCredentials.Insecure);

            var client = new GetItem.GetItemClient(channel);
            var res = client.Get(new ItemToRetrieve
            {
                Name = "yello1"
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
                Name = "yello1",
                Revision = 1
            });
            System.Console.WriteLine($"res:{res}");
            channel.ShutdownAsync().Wait();

        }
    }

    public class ItemRemove
    {
        public static void Run()
        {
            System.Console.WriteLine("yello");
            Channel channel = new Channel("127.0.0.1:5001", ChannelCredentials.Insecure);

            var client = new RemoveItem.RemoveItemClient(channel);
            var pong = client.Remove(new ItemToRemove
            {
                Name = "yello"
            });
            System.Console.WriteLine($"pong:{pong}");
            channel.ShutdownAsync().Wait();
        }
    }

    public class ItemAdder
    {
        public async static Task Run()
        {
            System.Console.WriteLine("Trying to add stuff");
            ICachy cachy = new CachyClient.Cachy();
            foreach (var item in Enumerable.Range(1, 60))
            {
                await cachy.Add($"test_{item.ToString()}", Encoding.UTF8.GetBytes($"yello_{item.ToString()}"));

            }
            System.Console.WriteLine($"Managed to add item ");
        }
    }
}
