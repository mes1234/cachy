using System;
using Xunit;
using Cachy.Common.Maybe;
using Cachy.Common.Validator;
using Cachy.Common.Converter;
using NSubstitute;


namespace Common.Tests
{
    public class MaybeTests
    {
        IValidator<string, string> validator = Substitute.For<IValidator<string, string>>();
        IConverter<string, string> converter = Substitute.For<IConverter<string, string>>();
        [Fact]
        public void MaybeTest_AlwaysValidates()
        {

            validator.Validate(Arg.Any<string>()).Returns(true);
            converter.Convert(Arg.Any<string>()).Returns(x => x[0]);

            var maybe = new Maybe<string, string>("dummy", validator, converter);

            Assert.True(maybe);
        }

        [Fact]
        public void MaybeTest_NeverValidates()
        {
            validator.Validate(Arg.Any<string>()).Returns(false);
            converter.Convert(Arg.Any<string>()).Returns(x => x[0]);

            var maybe = new Maybe<string, string>("dummy", validator, converter);

            Assert.False(maybe);
        }

        [Fact]
        public void MaybeTest_ValidationOkUseValue()
        {
            var input = "dummy";
            validator.Validate(Arg.Any<string>()).Returns(true);
            converter.Convert(Arg.Any<string>()).Returns(x => x[0]);

            var maybe = new Maybe<string, string>(input, validator, converter);

            string data = maybe;
            Assert.Equal(data, input);
        }
        [Fact]
        public void MaybeTest_ValidationFalseThrowsException()
        {
            var input = "dummy";
            validator.Validate(Arg.Any<string>()).Returns(false);
            converter.Convert(Arg.Any<string>()).Returns(x => x[0]);


            var maybe = new Maybe<string, string>(input, validator, converter);

            Assert.Throws<NotValidException>(() =>
            {
                string data = maybe;
            });

        }
    }
}
