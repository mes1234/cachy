namespace Cachy.Common.Converter
{
    public interface IConverter<T1, T2>
    {
        T2 Convert(T1 item);
    }
}