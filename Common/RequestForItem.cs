using System;

namespace Cachy.Common
{
    public record RequestForItem : IEntitie
    {
        public int Revision { get; init; }
        public string Name { get; init; }
    }
}