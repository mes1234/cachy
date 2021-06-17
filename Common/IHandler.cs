using System.Threading.Tasks;

namespace Cachy.Common
{
    public interface IHandler
    {
        Task Handle(IEntity item);
    }
}