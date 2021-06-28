using System;
using Cachy.Common;
namespace Cachy.Common.Converter
{
    public class SnapshotItemEntintyConverter : IConverter<ItemEntinty, SnapshotStorageItemEntinty>
    {

        public SnapshotStorageItemEntinty Convert(ItemEntinty item)
        {
            return new SnapshotStorageItemEntinty
            {
                Data = item.Data,
                Name = item.Name,
                Timestamp = item.Timestamp,
                TTL = item.TTL,
                Defined = true

            };
        }
    }
    public class SnapshotRequestForItemConverter : IConverter<RequestForItem, SnapshotRequestForItem>
    {

        public SnapshotRequestForItem Convert(RequestForItem item)
        {
            return new SnapshotRequestForItem
            {
                Name = item.Name,
                Result = item.Result,
                Revision = item.Revision,
                Defined = true
            };
        }
    }
}