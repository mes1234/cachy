using System.Threading.Tasks;
using Grpc.Core;
using Cachy.Events;
using Cachy.Common;

namespace Cachy.Communication.Services
{
    class InsertItemService : InsertItem.InsertItemBase
    {

        public override Task<Ack> InsertItem(Item request, ServerCallContext context)
        {
            ItemEntinty item = new()
            {
                Data = request.Data.ToByteArray(),
                Name = request.Name,
                Timestamp = System.DateTime.Now,
                TTL = (int)request.Ttl.Seconds,
            };

            return Task.FromResult(new Ack
            {
                Revision = 0
            });
        }
    }
}
