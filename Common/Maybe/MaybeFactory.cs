using System;

namespace Cachy.Common.Maybe
{
    public class MaybeFactory
    {
        public MaybeFactory(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; set; }

        public Maybe<T1, T2> GetMaybe<T1, T2>(T1 item)
        {
            var res = (Maybe<T1, T2>)this.ServiceProvider.GetService(typeof(Maybe<T1, T2>));
            res.Value = item;
            return res;
        }
    }
}