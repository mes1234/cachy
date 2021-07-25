using System.Collections.Generic;
using System;
using Cachy.Common.Converter;

namespace Cachy.Common.Maybe
{
    public class Maybe<T1, T2>
    {
        public T1 Value { private get; set; }

        private T2 Converted
        {
            get
            {
                return _converter.Convert(Value);
            }
        }

        private bool Valid
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

        private IEnumerable<IValidator<T1, T2>> _validators;

        private IConverter<T1, T2> _converter;

        public Maybe(IEnumerable<IValidator<T1, T2>> validators, IConverter<T1, T2> converter)
        {
            _validators = validators;
            _converter = converter;
        }

        public static implicit operator bool(Maybe<T1, T2> m) => m.Valid;

        public static implicit operator T2(Maybe<T1, T2> m) => (m.Valid == true)
              ? m.Converted
              : default(T2);
    }
}