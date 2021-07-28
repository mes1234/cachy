using Cachy.Common;

namespace Cachy.Storage.Snapshot.Validation.Inbound
{
    public class SnapshotRequestForItemValidator : IValidator<RequestForItem, SnapshotRequestForItem>
    {
        public bool Validate(RequestForItem obj)
        {
            return (obj.Revision > 0)
                ? false
                : true;
        }
    }
}