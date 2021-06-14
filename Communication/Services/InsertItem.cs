using System.Threading.Tasks;
using Grpc.Core;
using Cachy.Events;
using Cachy.Common;
using System.Collections.Concurrent;

namespace Cachy.Communication.Services
{
    class InsertItemService : InsertItem.InsertItemBase
    {
        private readonly ConcurrentQueue<IEntitie> _queue;
        public InsertItemService(ConcurrentQueue<IEntitie> Queue)
        {
            _queue = Queue;
        }
        public override Task<Ack> InsertItem(Item request, ServerCallContext context)
        {
            ItemEntinty item = new()
            {
                Data = request.Data.ToByteArray(),
                Name = request.Name,
                Timestamp = System.DateTime.Now,
                TTL = (int)request.Ttl.Seconds,
            };

            _queue.Enqueue(item);

            return Task.FromResult(new Ack
            {
                Revision = 0
            });
        }
    }
}
