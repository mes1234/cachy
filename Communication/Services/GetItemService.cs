using System.Collections.Concurrent;
using System;
using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf;
using Cachy.Events;
using Cachy.Common;

namespace Cachy.Communication.Services
{
    public class GetItemService : GetItem.GetItemBase
    {
        private readonly ConcurrentQueue<IEntity> _queue;

        public GetItemService(ConcurrentQueue<IEntity> queue)
        {
            _queue = queue;
        }

        public override async Task<RetrievedItem> Get(ItemToRetrieve request, ServerCallContext context)
        {
            // Prepare request for new item
            var item = new RequestForItem
            {
                Name = request.Name,
                Revision = request.Revision,
            };

            // Define start time and maximum allowed time for processing
            var start = DateTime.Now;
            var timeout = TimeSpan.FromSeconds(2000);

            // Start waiting loop
            item.Waiter = Task.Run(async () =>
             {
                 while (item.Result == null && ((DateTime.Now - start) < timeout))
                 {
                     await Task.Delay(10);
                 }
             });

            // Insert request to communication bus
            _queue.Enqueue(item);

            // Wait for response
            await item.Waiter;

            // Return response
            if (item.Result == null)
                return FailedResponse();
            else
                return HandleResponse(item.Result);
        }

        private RetrievedItem FailedResponse()
        {
            return new RetrievedItem
            {
                Item = new Item
                {
                    Name = "Not Found",
                },
            };
        }

        private RetrievedItem HandleResponse(object result)
        {
            var res = (ItemEntinty)result;

            if (res.Defined)
            {
                return new RetrievedItem
                {
                    Item = new Item
                    {
                        Data = ByteString.CopyFrom(res.Data),
                        Name = res.Name,
                        Ttl = new TimeToLive { Seconds = res.TTL },
                    },
                };
            }
            else
            {
                return new RetrievedItem
                {
                    Item = new Item(),
                };
            }
        }
    }
}