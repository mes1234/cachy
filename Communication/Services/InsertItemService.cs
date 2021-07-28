using System.Collections.Concurrent;
using System.Threading.Tasks;
using Grpc.Core;
using Cachy.Events;
using Cachy.Common;

namespace Cachy.Communication.Services
{
    public class InsertItemService : InsertItem.InsertItemBase
    {
        private readonly ConcurrentQueue<IEntity> _queue;

        public InsertItemService(ConcurrentQueue<IEntity> queue)
        {
            _queue = queue;
        }

        public override Task<Ack> InsertItem(Item request, ServerCallContext context)
        {
            ItemEntinty item = new ItemEntinty
            {
                Data = request.Data.ToByteArray(),
                Name = request.Name,
                Timestamp = System.DateTime.Now,
                TTL = (int)request.Ttl.Seconds,
            };

            _queue.Enqueue(item);

            return Task.FromResult(new Ack
            {
                Revision = 0,
            });
        }
    }
}
