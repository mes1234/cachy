using System.Threading.Tasks;
using Grpc.Core;
using Cachy.Events;

namespace Cachy.Communication.Services
{
    class PingPongService : PingPong.PingPongBase
    {

        public override Task<Pong> PingPong(Ping request, ServerCallContext context)
        {
            return Task.FromResult(new Pong { Message = "fddg" });
        }
    }
}
