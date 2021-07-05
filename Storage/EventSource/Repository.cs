
using System.Collections;
using System.Collections.Generic;
using Cachy.Common;

namespace Cachy.Storage.EventSource
{
    public interface IRepository<T> : IEnumerable<T>
        where T : IStoredEntity, new()
    {
        public void Add(T item);
        public void Remove(string name);
        public T Get(string name, int revison);
    }

    public class Repository<T> : IRepository<T>
        where T : IStoredEntity, new()
    {

        private readonly Dictionary<string, Events<T>> _registry = new();

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
            var lastItem = _registry[name][^1];
            var deletedItem = lastItem.CopyAndDeactivate();
            Add((T)deletedItem);

        }

        public T Get(string name, int revison)
        {
            if (!_registry.ContainsKey(name))
                return new T();

            if (_registry[name].Count < revison)
                return new T();

            return _registry[name][revison - 1];
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _registry)
            {
                yield return item.Value[item.Value.Count - 1];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => _registry.GetEnumerator();

    }
}
