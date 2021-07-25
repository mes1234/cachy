using System;
using Cachy.Common;

namespace Cachy.Common.Converter
{
    public class SnapshotItemToRemoveConverted : IConverter<ItemToRemoveEntity, ValidatedItemToRemoveEntity>
    {
        public ValidatedItemToRemoveEntity Convert(ItemToRemoveEntity item)
        {
            return new ValidatedItemToRemoveEntity
            {
                Name = item.Name,
            };
        }
    }
}