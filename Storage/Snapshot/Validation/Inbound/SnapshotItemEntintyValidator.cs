using Cachy.Common;

namespace Cachy.Storage.Snapshot.Validation.Inbound
{
    public class SnapshotItemEntintyValidator : IValidator<ItemEntinty, SnapshotStorageItemEntinty>
    {
        public bool Validate(ItemEntinty obj)
        {
            return true;
        }
    }
}