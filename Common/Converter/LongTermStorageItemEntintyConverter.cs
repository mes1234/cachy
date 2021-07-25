using System;
using Cachy.Common;

namespace Cachy.Common.Converter
{
    public class LongTermStorageItemEntintyConverter : IConverter<ItemEntinty, LongTermStorageItemEntinty>
    {
        public LongTermStorageItemEntinty Convert(ItemEntinty item)
        {
            return new LongTermStorageItemEntinty
            {
                Data = item.Data,
                Name = item.Name,
                Timestamp = item.Timestamp,
                TTL = item.TTL,
                Defined = true,
            };
        }
    }
}