using System.Collections.Generic;
using Cachy.Common;

namespace Cachy.Storage.EventSource
{

    public interface IEvents<T> : IList<T>
    where T : IStoredEntity
    { }

    public class Events<T> : List<T>, IEvents<T>
    where T : IStoredEntity
    {
        new public void Add(T item)
        {
            if (this.Count == 0)
                item.Revision = 1;
            else
                item.Revision = this.Count + 1;
            base.Add(item);
        }
    }
}