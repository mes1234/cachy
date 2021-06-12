using System.Threading.Tasks;

namespace Cachy.Common
{
    public interface IHandler<T>
    {
        Task Handle(T item);
    }
}