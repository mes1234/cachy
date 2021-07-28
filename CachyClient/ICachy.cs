using System.Threading.Tasks;

namespace CachyClient
{
    public interface ICachy
    {
        Task Add(string name, byte[] paylod, int ttl = 60 * 60);

        Task<byte[]> Get(string name);

        Task<byte[]> Get(string name, int revision);

        Task Remove(string name);
    }
}