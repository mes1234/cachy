using Cachy.Common;
namespace Cachy.Storage.LongTerm.Validation.Inbound
{
    public class LTSItemEntintyValidator : IValidator<ItemEntinty, LongTermStorageItemEntinty>
    {
        public bool Validate(ItemEntinty obj)
        {
            return true;
        }
    }
}