using System.Threading.Tasks;
using Grpc.Core;
using Google.Protobuf;
using Cachy.Events;
using Cachy.Common;
using System.Collections.Concurrent;
using System.Threading;
using System;
using Google.Protobuf.WellKnownTypes;

namespace Cachy.Communication.Services
{
    class RemoveItemService : RemoveItem.RemoveItemBase
    {
        private readonly ConcurrentQueue<IEntity> _queue;
        public RemoveItemService(ConcurrentQueue<IEntity> Queue)
        {
            _queue = Queue;
        }


        public override Task<Empty> Remove(ItemToRemove request, ServerCallContext context)
        {

            var item = new ItemToRemoveEntity
            {
                Defined = true,
                Name = request.Name

            };
            // Insert request to communication bus
            _queue.Enqueue(item);

            return Task.FromResult(new Empty());

        }
    }
}
