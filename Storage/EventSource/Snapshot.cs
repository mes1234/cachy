using System.Collections.Generic;
using Cachy.Common;

namespace Cachy.Storage.EventSource
{
    public class Snapshot<T>
      where T : IEntity
    {
        private readonly Dictionary<string, T> registry = new();

        public void Add(T item)
        {
            registry[item.Name] = item;
        }

        public T Get(string name)
        {
            if (!registry.ContainsKey(name))
                return default(T);

            return registry[name];
        }
    }
}