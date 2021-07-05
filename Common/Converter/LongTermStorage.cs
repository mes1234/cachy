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
                Defined = true

            };
        }
    }
    public class LongTermStorageRequestConverter : IConverter<RequestForItem, LongTermStorageRequestForItem>
    {

        public LongTermStorageRequestForItem Convert(RequestForItem item)
        {
            return new LongTermStorageRequestForItem
            {
                Name = item.Name,
                Result = item.Result,
                Revision = item.Revision
            };
        }
    }

    public class ValidatedItemToRemoveConverter : IConverter<ItemToRemoveEntity, ValidatedItemToRemoveEntity>
    {
        public ValidatedItemToRemoveEntity Convert(ItemToRemoveEntity item)
        {
            return new ValidatedItemToRemoveEntity
            {
                Defined = item.Defined,
                Name = item.Name
            };
        }
    }
}