namespace Cachy.Common.Converter
{
    public interface IConverter<T, U>
    {
        U Convert(T item);
    }
}