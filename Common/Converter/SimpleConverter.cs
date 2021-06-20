using System;
using Cachy.Common;
namespace Cachy.Common.Converter
{

    public class SimpleConverter : IConverter<ItemEntinty, LongTermStorageItemEntinty>
    {

        public LongTermStorageItemEntinty Convert(ItemEntinty item)
        {
            return new LongTermStorageItemEntinty
            {
                Data = item.Data,
                Name = item.Name,
                Timestamp = item.Timestamp,
                TTL = item.TTL
            };
        }
    }
    public class SimpleRequestConverter : IConverter<RequestForItem, RequestForItemValidated>
    {

        public RequestForItemValidated Convert(RequestForItem item)
        {
            return new RequestForItemValidated
            {
                Name = item.Name,
                Result = item.Result,
                Revision = item.Revision
            };
        }
    }
}