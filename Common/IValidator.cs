namespace Cachy.Common
{

    public interface IValidator<T, U>
    {
        public bool Validate(T obj);
    }


}