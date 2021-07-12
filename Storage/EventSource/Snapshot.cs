using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cachy.Common;

namespace Cachy.Storage.EventSource
{
    public class Snapshot<T>
      where T : IEntity, new()
    {
        private readonly Dictionary<string, T> _registry = new();

        public void Add(T item)
        {
            lock (_registry)
            {
                _registry[item.Name] = item;
            }
        }

        public void CheckTtl()
        {
            if (_registry.Count == 0) return;
            var random = new Random();

            T item;
            lock (_registry)
            {
                // get  item for random Key
                item = _registry.ElementAt(random.Next(0, _registry.Count)).Value;

            }

            if ((DateTime.Now - item.Timestamp).TotalSeconds > item.TTL) // TODO why this is setting Active to false?
            {
                Remove(item.Name);
            }
        }

        public T Get(string name)
        {
            if (!_registry.ContainsKey(name))
                return new T();

            return _registry[name];
        }


        public void Remove(string name)
        {

            if (!_registry.ContainsKey(name))
                return;
            lock (_registry)
            {
                _registry.Remove(name);
            }

        }

    }
}