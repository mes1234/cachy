using System.Collections;
using System.Collections.Generic;
using Cachy.Common;

namespace Cachy.Storage.EventSource
{
    public class Snapshot<T> : IEnumerable<T>
      where T : IEntity, new()
    {
        private readonly Dictionary<string, T> registry = new();

        public void Add(T item)
        {
            registry[item.Name] = item;
        }

        public T Get(string name)
        {
            if (!registry.ContainsKey(name))
                return new T();

            return registry[name];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.registry.Values.GetEnumerator();
        }

        public void Remove(string name)
        {
            if (!registry.ContainsKey(name))
                return;
            registry.Remove(name);

        }

        IEnumerator IEnumerable.GetEnumerator() => registry.GetEnumerator();
    }
}