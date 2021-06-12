
using System.Collections.Generic;
using Cachy.Common;

namespace Cachy.Storage.EventSource
{
    public class Repository<T>
        where T : IEntitie
    {
        private readonly Dictionary<string, List<T>> registry = new();

        public void Add(T item)
        {
            if (registry.ContainsKey(item.Name))
            {
                registry[item.Name].Add(item);
            }
            else
            {
                registry[item.Name] = new List<T> { item };
            }
        }

        public T Get(string name, int revison)
        {
            if (!registry.ContainsKey(name))
                return default(T);

            if (registry[name].Count < revison)
                return default(T);

            return registry[name][revison];
        }
    }
}
