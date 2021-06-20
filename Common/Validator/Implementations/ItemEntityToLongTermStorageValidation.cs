using Cachy.Common;
namespace Cachy.Common.Validator.Implementation
{
    public class LongTermStorageItemEntintyValidator : IValidator<ItemEntinty, LongTermStorageItemEntinty>
    {
        public bool Validate(ItemEntinty obj)
        {
            return true;
        }
    }
}