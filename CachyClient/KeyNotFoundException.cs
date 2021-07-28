using System;

namespace CachyClient.Common
{
    public class KeyNotFoundException : Exception
    {
        public KeyNotFoundException()
        {
        }

        public KeyNotFoundException(string message)
            : base(message)
        {
        }

        public KeyNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}