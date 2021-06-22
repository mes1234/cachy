using Cachy.Common;
namespace Cachy.Storage.LongTerm.Validation.Inbound
{
    public class LTSRequestForItemValidator : IValidator<RequestForItem, LongTermStorageRequestForItem>
    {
        public bool Validate(RequestForItem obj)
        {
            return (obj.Revision > 0)
                ? true
                : false;
        }
    }
}