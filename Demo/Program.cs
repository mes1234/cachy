using System;
using CachyClient;
using System.Threading.Tasks;
using System.Linq;

namespace Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Begin Demo");

            ICachy cachy = new CachyClient.Cachy();

            await AddAndGetSingle(cachy);

            await AddAndGetOlderVersion(cachy);

            await TryToGetMissingValue(cachy);

            await TryToGetMissingrEvisionValue(cachy);

            await TryToGetAfterTTLExpired(cachy);

            await GetSnapshotAndThenOlderVerision(cachy);

            await PerformanceTest(cachy);

        }

        async static Task AddAndGetSingle(ICachy cachy)
        {
            System.Console.WriteLine("Demo add and retrieve single item");

            await Add(cachy, "demo1", "content1");

            await Retrieve(cachy, "demo1", 0);

            System.Console.WriteLine("End this demo \n---------------------\n");
        }

        async static Task AddAndGetOlderVersion(ICachy cachy)
        {
            System.Console.WriteLine("Demo add and retrieve single item with different versions");

            foreach (var revision in Enumerable.Range(1, 10))
            {
                await Add(cachy, "demo2", $"content_{revision}");
            }


            foreach (var revision in Enumerable.Range(1, 10))
            {
                await Retrieve(cachy, "demo2", revision);
            }


            System.Console.WriteLine("End this demo \n---------------------\n");
        }

        async static Task TryToGetMissingValue(ICachy cachy)
        {
            System.Console.WriteLine("Demo try to retrieve missing value");
            await Retrieve(cachy, "demo3", 0);
            System.Console.WriteLine("End this demo \n---------------------\n");
        }

        async static Task TryToGetMissingrEvisionValue(ICachy cachy)
        {
            System.Console.WriteLine("Demo try to get not found revision");
            foreach (var revision in Enumerable.Range(1, 10))
            {
                await Add(cachy, "demo4", $"content_{revision}");
            }
            await Retrieve(cachy, "demo4", 1000);
            System.Console.WriteLine("End this demo \n---------------------\n");
        }

        async static Task TryToGetAfterTTLExpired(ICachy cachy)
        {
            System.Console.WriteLine("Demo try to get after ttl expired");
            await Add(cachy, "demo5", $"content5", 3);

            await Retrieve(cachy, "demo5", 0);

            await Task.Delay(10 * 1000);

            await Retrieve(cachy, "demo5", 0);

            System.Console.WriteLine("End this demo \n---------------------\n");
        }

        async static Task GetSnapshotAndThenOlderVerision(ICachy cachy)
        {
            System.Console.WriteLine("Demo try to get last snapshot and later older version");
            await Add(cachy, "demo6", $"content6_1", 30);
            await Add(cachy, "demo6", $"content6_2", 30);

            await Retrieve(cachy, "demo6", 0);

            await Retrieve(cachy, "demo6", 1);

            System.Console.WriteLine("End this demo \n---------------------\n");
        }

        async static Task Add(ICachy cachy, string name, string item, int ttl = 3600, bool silent = false)
        {
            if (!silent)
                System.Console.WriteLine("Add single item");

            await cachy.Add(name, System.Text.Encoding.UTF8.GetBytes(item), ttl);

        }

        async static Task Remove(ICachy cachy, string name)
        {
            System.Console.WriteLine("Remove single item");
            await cachy.Remove(name);
        }

        async static Task Retrieve(ICachy cachy, string name, int revison, bool silent = false)
        {
            try
            {
                if (!silent)
                    System.Console.WriteLine($"Try to get {name} version {revison}");
                var item = System.Text.Encoding.UTF8.GetString(await cachy.Get(name, revison));
                if (!silent)
                    System.Console.WriteLine($"I've recieved {item}");
            }
            catch (System.Exception)
            {
                System.Console.WriteLine($"Nothing for {name} :(");
            }
        }

        async static Task PerformanceTest(ICachy cachy)
        {
            System.Console.WriteLine("Demo add 1k items");
            var count = 1000;
            foreach (var revision in Enumerable.Range(1, count))
            {
                await Add(cachy, "demo7", $"content_{revision}", 3600, true);
            }


            foreach (var revision in Enumerable.Range(1, count))
            {
                await Retrieve(cachy, "demo7", revision);
            }


            System.Console.WriteLine("End this demo \n---------------------\n");
        }
    }
}
