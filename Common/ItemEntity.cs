using System;

namespace Cachy.Common
{
    public record ItemEntinty : IEntity
    {
        public DateTime Timestamp { get; init; }
        public string Name { get; init; }
        public int TTL { get; init; }
        public byte[] Data { get; init; }
    }

    public record LongTermStorageItemEntinty : ItemEntinty
    {

    }
    public record StoredItemEntity : ItemEntinty, IStoredEntity
    {
        public int Revision { get; set; }
    }
}
