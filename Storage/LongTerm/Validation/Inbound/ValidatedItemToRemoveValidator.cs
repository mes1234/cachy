using Cachy.Common;

namespace Cachy.Storage.LongTerm.Validation.Inbound
{
    public class ValidatedItemToRemoveValidator : IValidator<ItemToRemoveEntity, ValidatedItemToRemoveEntity>
    {
        public bool Validate(ItemToRemoveEntity obj)
        {
            return string.IsNullOrEmpty(obj.Name)
                ? false
                : true;
        }
    }
}