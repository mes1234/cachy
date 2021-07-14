using System;
using CachyClient;
using System.Threading;
using System.Threading.Tasks;

namespace Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello");
            ICachy cachy = new CachyClient.Cachy();

            var item1 = "item1";

            await cachy.Add("item1", System.Text.Encoding.UTF8.GetBytes(item1));

            Thread.Sleep(1000);

            var item1_retrieved = System.Text.Encoding.UTF8.GetString(await cachy.Get("item1"));

            System.Console.WriteLine($"I've recieved {item1_retrieved}");

            await cachy.Remove("item1");

            try
            {
                var item1_retrieved2 = System.Text.Encoding.UTF8.GetString(await cachy.Get("item1"));
                // IT SHOULD THROW
                System.Console.WriteLine($"I've recieved {item1_retrieved2}");
            }
            catch (System.Exception)
            {

                System.Console.WriteLine($"Nothing for item1 :(");
            }

        }
    }
}
