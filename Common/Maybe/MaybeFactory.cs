using System;

namespace Cachy.Common.Maybe
{
    public class MaybeFactory
    {
        public IServiceProvider _serviceProvider { get; set; }
        public MaybeFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Maybe<T, U> GetMaybe<T, U>(T item)
        {
            var res = (Maybe<T, U>)_serviceProvider.GetService(typeof(Maybe<T, U>));
            res.Value = item;
            return res;
        }
    }
}