
using System.Threading.Tasks;
using Grpc.Core;

namespace Communication
{
    class PingPongImpl : Communication.PingPong.PingPongBase
    {

        public override Task<Pong> PingPong(Ping request, ServerCallContext context)
        {
            return Task.FromResult(new Pong { Message = "fddg" });
        }
    }
}
