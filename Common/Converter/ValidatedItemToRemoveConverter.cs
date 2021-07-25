using System;
using Cachy.Common;

namespace Cachy.Common.Converter
{
    public class ValidatedItemToRemoveConverter : IConverter<ItemToRemoveEntity, ValidatedItemToRemoveEntity>
    {
        public ValidatedItemToRemoveEntity Convert(ItemToRemoveEntity item)
        {
            return new ValidatedItemToRemoveEntity
            {
                Defined = item.Defined,
                Name = item.Name,
            };
        }
    }
}
