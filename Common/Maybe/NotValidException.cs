using System;

namespace Cachy.Common.Maybe
{
    public class NotValidException : Exception
    {
        public NotValidException(string message)
            : base(message)
        {
        }
    }
}