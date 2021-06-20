namespace Cachy.Common.Validator
{

    public interface IValidator<T, U>
    {
        public bool Validate(T obj);
    }


}