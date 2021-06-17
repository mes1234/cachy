
using System.Collections.Generic;
using Cachy.Common;

namespace Cachy.Storage.EventSource
{
    public class Repository<T>
        where T : IStoredEntity
    {

        private readonly Dictionary<string, IEvents<T>> _registry;

        public Repository(Dictionary<string, IEvents<T>> Registry)
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

        public T Get(string name, int revison)
        {
            if (!_registry.ContainsKey(name))
                return default(T);

            if (_registry[name].Count < revison)
                return default(T);

            return _registry[name][revison];
        }
    }
}
