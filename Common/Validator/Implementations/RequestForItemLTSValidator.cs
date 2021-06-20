using Cachy.Common;
namespace Cachy.Common.Validator.Implementation
{
    public class LongTermStorageItemRequestForItem : IValidator<RequestForItem, RequestForItemValidated>
    {


        public bool Validate(RequestForItem obj)
        {
            return (obj.Revision > 0)
                ? true
                : false;
        }
    }
}