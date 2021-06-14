using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf;
using Cachy.Events;
using Cachy.Common;
using System.Collections.Concurrent;

namespace Cachy.Communication.Services
{
    class GetItemService : GetItem.GetItemBase
    {
        private readonly ConcurrentQueue<IEntitie> _queue;
        public GetItemService(ConcurrentQueue<IEntitie> Queue)
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
                await Task.Delay(10);
            }

            var res = (ItemEntinty)item.Result;
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
    }
}
