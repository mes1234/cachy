using System;
using System.Collections.Generic;
using System.Linq;
using Cachy.Common;

namespace Cachy.Storage.Persistance
{
    public interface IRepository<T>
        where T : IStoredEntity, new()
    {
        public void Add(T item);

        public void Remove(string name);

        public T Get(string name, int revison);

        public void CheckTtl();
    }

    public class Repository<T> : IRepository<T>
        where T : IStoredEntity, new()
    {
        private readonly Dictionary<string, Events<T>> _registry = new Dictionary<string, Events<T>>();

        public void Add(T item)
        {
            lock (_registry)
            {
                if (_registry.ContainsKey(item.Name))
                    _registry[item.Name].Add(item);
                else
                    _registry[item.Name] = new Events<T> { item };
            }
        }

        public void Remove(string name)
        {
            if (!_registry.ContainsKey(name))
                return;
            IStoredEntity deletedItem;
            lock (_registry)
            {
                var lastItem = _registry[name][^1];
                deletedItem = lastItem.CopyAndDeactivate();
            }

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

        public void CheckTtl()
        {
            if (_registry.Count == 0)
                return;

            var random = new Random();

            Events<T> itemCollection;

            // get all items for random Key
            lock (_registry)
            {
                itemCollection = _registry.ElementAt(random.Next(0, _registry.Count)).Value;
            }

            // get last revision
            var item = itemCollection[itemCollection.Count - 1];
            if ((DateTime.Now - item.Timestamp).TotalSeconds > item.TTL &&
                        item.Active)
            {
                Remove(item.Name);
            }
        }
    }
}
