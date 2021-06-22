using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf;
using Cachy.Events;
using Cachy.Common;
using System.Collections.Concurrent;
using System.Threading;

namespace Cachy.Communication.Services
{
    class GetItemService : GetItem.GetItemBase
    {
        private readonly ConcurrentQueue<IEntity> _queue;
        public GetItemService(ConcurrentQueue<IEntity> Queue)
        {
            _queue = Queue;
        }

        public override async Task<RetrievedItem> Get(ItemToRetrieve request, ServerCallContext context)
        {

            var item = new RequestForItem
            {
                Name = request.Name,
                Revision = request.Revision,
            };

            _queue.Enqueue(item);

            while (item.Result == null)
            {
                await Task.Delay(0);
            }

            var res = (ItemEntinty)item.Result;
            if (res.Defined)
            {
                return new RetrievedItem
                {
                    Item = new Item
                    {
                        Data = ByteString.CopyFrom(res.Data),
                        Name = res.Name,
                        Ttl = new TimeToLive { Seconds = res.TTL }
                    }
                };
            }
            else
            {
                return new RetrievedItem
                {
                    Item = new Item
                    {
                        Data = ByteString.CopyFrom(new byte[] { 0 }),
                        Name = "Not Found",
                        Ttl = new TimeToLive { Seconds = 0 },

                    }
                };
            }
        }
    }
}
