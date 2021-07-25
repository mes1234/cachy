namespace Cachy.Common
{
    public interface IValidator<T1, T2>
    {
        public bool Validate(T1 obj);
    }
}