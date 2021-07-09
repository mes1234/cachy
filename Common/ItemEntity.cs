using System;

namespace Cachy.Common
{
    public record ItemEntinty : IEntity
    {
        public DateTime Timestamp { get; init; }
        public string Name { get; init; }
        public int TTL { get; init; }
        public byte[] Data { get; init; }
        public bool Defined { get; init; }
        public bool Active { get; init; }
    }

    public record ItemToRemoveEntity : IEntity
    {
        public string Name { get; init; }
        public bool Defined { get; init; }
        public bool Active { get; init; }
        public DateTime Timestamp { get; init; }
        public int TTL { get; init; }
    }

    public record ValidatedItemToRemoveEntity : ItemToRemoveEntity { }
    public record LongTermStorageItemEntinty : ItemEntinty { }
    public record SnapshotStorageItemEntinty : ItemEntinty { }
    public record StoredItemEntity : ItemEntinty, IStoredEntity
    {
        public int Revision { get; set; }

        public IStoredEntity CopyAndDeactivate()
        {
            return this with { Active = false };
        }
    }
}
