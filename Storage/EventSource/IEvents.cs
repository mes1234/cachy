using System.Collections.Generic;
using Cachy.Common;

namespace Cachy.Storage.EventSource
{


    public class Events<T> : List<T>
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