using System.Threading.Tasks;
using Grpc.Core;
using Cachy.Events;
using Cachy.Common;
using System.Collections.Concurrent;

namespace Cachy.Communication.Services
{
    class GetItemService : GetItem.GetItemBase
    {
        private readonly ConcurrentQueue<RequestForItem> _queue;
        public GetItemService(ConcurrentQueue<RequestForItem> Queue)
        {
            _queue = Queue;
        }

        public override Task<RetrievedItem> Get(ItemToRetrieve request, ServerCallContext context)
        {
            var item = new RequestForItem
            {
                Name = request.Name,
                Revision = request.Revision
            };
            _queue.Enqueue(item);

            return Task.FromResult(new RetrievedItem
            {
                Item = new Item(),
                Revision = 1
            });
        }
    }
}
