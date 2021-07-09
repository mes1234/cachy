using System;
using System.Threading.Tasks;

namespace Cachy.Common
{
    public record RequestForItem : IEntity
    {
        public int Revision { get; init; }
        public string Name { get; init; }
        public object Result { get; set; }
        public Task Waiter { get; set; }
        public bool Defined { get; init; }
        public bool Active { get; init; }
        public DateTime Timestamp { get; init; }
        public int TTL { get; init; }
    }

    public record LongTermStorageRequestForItem : RequestForItem { }
    public record SnapshotRequestForItem : RequestForItem { }
}