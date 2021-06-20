using System;
using Cachy.Common.Validator;
using Cachy.Common.Converter;

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
                return _validator.Validate(this.Value);
            }
        }
        private IValidator<T, U> _validator;
        private IConverter<T, U> _converter;
        public Maybe(IValidator<T, U> validator, IConverter<T, U> converter)
        {
            _validator = validator;
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