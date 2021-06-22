using System;
using Cachy.Common;
namespace Cachy.Common.Converter
{

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