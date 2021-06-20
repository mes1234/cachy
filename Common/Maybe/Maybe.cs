using System;
using Cachy.Common.Validator;
using Cachy.Common.Converter;
using System.Collections.Generic;

namespace Cachy.Common.Maybe
{
    public class Maybe<T, U>
    {
        public T Value { private get; set; }
        private U _converted
        {
            get
            {
                return _converter.Convert(Value);
            }
        }
        private bool _valid
        {
            get
            {
                foreach (var validator in _validators)
                {
                    if (!validator.Validate(this.Value))
                        return false;
                }
                return true;
            }
        }


        private IEnumerable<IValidator<T, U>> _validators;
        private IConverter<T, U> _converter;
        public Maybe(IEnumerable<IValidator<T, U>> validators, IConverter<T, U> converter)
        {
            _validators = validators;
            _converter = converter;
        }
        public static implicit operator bool(Maybe<T, U> m) => m._valid;
        public static implicit operator U(Maybe<T, U> m) => (m._valid == true)
              ? m._converted
              : throw new NotValidException($"Validation for entity {m.Value} failed");
    }


    public class NotValidException : Exception
    {
        public NotValidException(string message) : base(message)
        {
        }
    }
}