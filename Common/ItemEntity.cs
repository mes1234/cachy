using System;

namespace Cachy.Common
{
    public record ItemEntinty
    {
        public DateTime Timestamp { get; init; }
        public string Name { get; init; }
        public int TTL { get; init; }
        public byte[] Data { get; init; }
    }
}
