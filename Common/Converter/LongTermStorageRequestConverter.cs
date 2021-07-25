using System;
using Cachy.Common;

namespace Cachy.Common.Converter
{
    public class LongTermStorageRequestConverter : IConverter<RequestForItem, LongTermStorageRequestForItem>
    {
        public LongTermStorageRequestForItem Convert(RequestForItem item)
        {
            return new LongTermStorageRequestForItem
            {
                Name = item.Name,
                Result = item.Result,
                Revision = item.Revision,
            };
        }
    }
}
