using System;

namespace Cachy.Common
{

    public interface IEntity
    {
        public bool Active { get; init; }
        public DateTime Timestamp { get; init; }
        public int TTL { get; init; }
        public string Name { get; init; }
        public bool Defined { get; init; }
    }
}