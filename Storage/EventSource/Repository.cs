
using System.Collections.Generic;
using Cachy.Common;

namespace Cachy.Storage.EventSource
{
    public class Repository<T>
        where T : IStoredEntity, new()
    {

        private readonly Dictionary<string, Events<T>> _registry;

        public Repository(Dictionary<string, Events<T>> Registry)
        {
            _registry = Registry;
        }



        public void Add(T item)
        {
            if (_registry.ContainsKey(item.Name))
                _registry[item.Name].Add(item);
            else
                _registry[item.Name] = new Events<T> { item };
        }

        public void Remove(string name)
        {
            if (!_registry.ContainsKey(name))
                return;
            // TODO time for ES

        }

        public T Get(string name, int revison)
        {
            if (!_registry.ContainsKey(name))
                return new T();

            if (_registry[name].Count < revison)
                return new T();

            return _registry[name][revison - 1];
        }
    }
}
