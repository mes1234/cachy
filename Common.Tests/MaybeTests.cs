using System;
using Xunit;
using Cachy.Common;
using Cachy.Common.Maybe;
using Cachy.Common.Converter;
using NSubstitute;
using System.Collections.Generic;

namespace Common.Tests
{
    public class MaybeTests
    {
        IValidator<string, string> validator = Substitute.For<IValidator<string, string>>();


        IConverter<string, string> converter = Substitute.For<IConverter<string, string>>();


        private IEnumerable<T> EnumarableThis<T>(T item) => new List<T> { item };

        [Fact]
        public void MaybeTest_AlwaysValidates()
        {

            validator.Validate(Arg.Any<string>()).Returns(true);
            converter.Convert(Arg.Any<string>()).Returns(x => x[0]);

            var maybe = new Maybe<string, string>(EnumarableThis(validator), converter);
            maybe.Value = "dummy";
            Assert.True(maybe);
        }

        [Fact]
        public void MaybeTest_NeverValidates()
        {
            validator.Validate(Arg.Any<string>()).Returns(false);
            converter.Convert(Arg.Any<string>()).Returns(x => x[0]);

            var maybe = new Maybe<string, string>(EnumarableThis(validator), converter);
            maybe.Value = "dummy";
            Assert.False(maybe);
        }

        [Fact]
        public void MaybeTest_ValidationOkUseValue()
        {
            var input = "dummy";
            validator.Validate(Arg.Any<string>()).Returns(true);
            converter.Convert(Arg.Any<string>()).Returns(x => x[0]);

            var maybe = new Maybe<string, string>(EnumarableThis(validator), converter);
            maybe.Value = input;
            string data = maybe;
            Assert.Equal(data, input);
        }
        [Fact]
        public void MaybeTest_ValidationFalseThrowsException()
        {
            var input = "dummy";
            validator.Validate(Arg.Any<string>()).Returns(false);
            converter.Convert(Arg.Any<string>()).Returns(x => x[0]);


            var maybe = new Maybe<string, string>(EnumarableThis(validator), converter);
            maybe.Value = input;
            Assert.Throws<NotValidException>(() =>
            {
                string data = maybe;
            });

        }
    }
}
