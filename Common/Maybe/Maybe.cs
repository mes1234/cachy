using System;
using Cachy.Common.Validator;
using Cachy.Common.Converter;

namespace Cachy.Common.Maybe
{
    public class Maybe<T, U>
    {
        private T _value { get; init; }
        private U _converted
        {
            get
            {
                return _converter.Convert(_value);
            }
        }
        private bool _valid
        {
            get
            {
                return _validator.Validate(this._value);
            }
        }
        private IValidator<T, U> _validator;
        private IConverter<T, U> _converter;
        public Maybe(T value, IValidator<T, U> validator, IConverter<T, U> converter)
        {
            _value = value;
            _validator = validator;
            _converter = converter;
        }
        public static implicit operator bool(Maybe<T, U> m) => m._valid;
        public static implicit operator U(Maybe<T, U> m) => (m._valid == true)
              ? m._converted
              : throw new NotValidException($"Validation for entity {m._value} failed");
    }


    public class NotValidException : Exception
    {
        public NotValidException(string message) : base(message)
        {
        }
    }
}