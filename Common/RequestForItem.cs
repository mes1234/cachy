using System;
using System.Threading.Tasks;

namespace Cachy.Common
{
    public record RequestForItem : IEntitie
    {
        public int Revision { get; init; }
        public string Name { get; init; }
        public object Result { get; set; }
    }
}