using System.Threading.Tasks;
using System.Collections.Concurrent;
using Grpc.Core;
using Cachy.Events;
using Cachy.Common;
using Google.Protobuf.WellKnownTypes;

namespace Cachy.Communication.Services
{
    public class RemoveItemService : RemoveItem.RemoveItemBase
    {
        private readonly ConcurrentQueue<IEntity> _queue;

        public RemoveItemService(ConcurrentQueue<IEntity> queue)
        {
            _queue = queue;
        }

        public override Task<Empty> Remove(ItemToRemove request, ServerCallContext context)
        {
            var item = new ItemToRemoveEntity
            {
                Defined = true,
                Name = request.Name,
            };

            // Insert request to communication bus
            _queue.Enqueue(item);

            return Task.FromResult(new Empty());
        }
    }
}
