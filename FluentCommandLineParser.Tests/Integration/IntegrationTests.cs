#region License
// IntegrationTests.cs
// Copyright (c) 2013, Simon Williams
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provide
// d that the following conditions are met:
// 
// Redistributions of source code must retain the above copyright notice, this list of conditions and the
// following disclaimer.
// 
// Redistributions in binary form must reproduce the above copyright notice, this list of conditions and
// the following disclaimer in the documentation and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED 
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
// PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
#endregion

using Fclp.Tests.FluentCommandLineParser;
using Fclp.Tests.Internals;
using Xunit;

namespace Fclp.Tests.Integration
{
    public class IntegrationTests
    {
        [Theory]
        [InlineData("-b", true)]
        [InlineData("-b+", true)]
        [InlineData("-b-", false)]
        [InlineData("/b:true", true)]
        [InlineData("/b:false", false)]
        [InlineData("-b true", true)]
        [InlineData("-b false", false)]
        [InlineData("-b=true", true)]
        [InlineData("-b=false", false)]
        [InlineData("-b on", true)]
        [InlineData("-b off", false)]
        [InlineData("-b ON", true)]
        [InlineData("-b OFF", false)]
        [InlineData("-b:on", true)]
        [InlineData("-b:off", false)]
        [InlineData("-b=on", true)]
        [InlineData("-b=off", false)]
        public void SimpleShortOptionsAreParsedCorrectlyBool(string arguments, bool expectedBoolean)
        {
            var sut = new Fclp.FluentCommandLineParser();
            bool? actualBoolean = null;
            sut.Setup<bool>('b').Callback(b => actualBoolean = b);
            var args = arguments.ParseArguments();
            var results = sut.Parse(args);
            Assert.False(results.HasErrors);
            Assert.Equal(actualBoolean, expectedBoolean);
        }

        [Theory]
        [InlineData("-s {0}", "Hello World")]
        [InlineData("-s:{0}", "Hello World")]
        [InlineData("-s={0}", "Hello World")]
        public void SimpleShortOptionsAreParsedCorrectlyString(string arguments, string expectedString)
        {
            var sut = new Fclp.FluentCommandLineParser();
            string actualString = null;
            sut.Setup<string>('s').Callback(s => actualString = s);
            var args = string.Format(arguments, "\"" + expectedString + "\"").ParseArguments();
            var results = sut.Parse(args);
            Assert.False(results.HasErrors);
            Assert.Equal(actualString, expectedString);
        }

        [Theory]
        [InlineData("-i 123", 123)]
        [InlineData("-i:123", 123)]
        [InlineData("-i=123", 123)]
        public void SimpleShortOptionsAreParsedCorrectlyInt(string arguments, int expectedInt32)
        {
            var sut = new Fclp.FluentCommandLineParser();
            int? actualInt32 = null;
            sut.Setup<int>('i').Callback(i => actualInt32 = i);
            var args = arguments.ParseArguments();
            var results = sut.Parse(args);
            Assert.False(results.HasErrors);
            Assert.Equal(actualInt32, expectedInt32);
        }


        [Theory]
        [InlineData("-l 2147483649", 2147483649)]
        [InlineData("-l:2147483649", 2147483649)]
        [InlineData("-l=2147483649", 2147483649)]
        public void SimpleShortOptionsAreParsedCorrectlyLong(string arguments, long expectedInt64)
        {
            var sut = new Fclp.FluentCommandLineParser();
            long? actualInt64 = null;
            sut.Setup<long>('l').Callback(l => actualInt64 = l);
            var args = arguments.ParseArguments();
            var results = sut.Parse(args);
            Assert.False(results.HasErrors);
            Assert.Equal(actualInt64, expectedInt64);
        }

        [Theory]
        [InlineData("-d 123.456", 123.456)]
        [InlineData("-d:123.456", 123.456)]
        [InlineData("-d=123.456", 123.456)]
        public void SimpleShortOptionsAreParsedCorrectlyDouble(string arguments, double expectedDouble)
        {
            var sut = new Fclp.FluentCommandLineParser();
            double? actualDouble = null;
            sut.Setup<double>('d').Callback(d => actualDouble = d);
            var args = arguments.ParseArguments();
            var results = sut.Parse(args);
            Assert.False(results.HasErrors);
            Assert.Equal(actualDouble, expectedDouble);
        }

        [Theory]
        [InlineData("-e 1", TestEnum.Value1)]
        [InlineData("-e:1", TestEnum.Value1)]
        [InlineData("-e=1", TestEnum.Value1)]
        [InlineData("-e Value1", TestEnum.Value1)]
        [InlineData("-e:Value1", TestEnum.Value1)]
        [InlineData("-e=Value1", TestEnum.Value1)]
        public void SimpleShortOptionsAreParsedCorrectlyEnum(string arguments, TestEnum expectedEnum)
        {
            var sut = new Fclp.FluentCommandLineParser();
            TestEnum? actualEnum = null;
            sut.Setup<TestEnum>('e').Callback(d => actualEnum = d);
            var args = arguments.ParseArguments();
            var results = sut.Parse(args);
            Assert.False(results.HasErrors);
            Assert.Equal(actualEnum, expectedEnum);
        }


        [Theory]
        [InlineData("-xyz", true)]
        [InlineData("-xyz+", true)]
        [InlineData("-xyz-", false)]
        public void combined_bool_short_options_should_be_parsed_correctly(string arguments, bool expectedValue)
        {
            var sut = new Fclp.FluentCommandLineParser();

            bool? actualXValue = null;
            bool? actualYValue = null;
            bool? actualZValue = null;

            sut.Setup<bool>('x').Callback(x => actualXValue = x);
            sut.Setup<bool>('y').Callback(y => actualYValue = y);
            sut.Setup<bool>('z').Callback(z => actualZValue = z);

            var args = arguments.ParseArguments();

            var results = sut.Parse(args);

            Assert.False(results.HasErrors);

            Assert.True(actualXValue.HasValue);
            Assert.True(actualYValue.HasValue);
            Assert.True(actualZValue.HasValue);

            Assert.Equal(actualXValue.Value, expectedValue);
            Assert.Equal(actualYValue.Value, expectedValue);
            Assert.Equal(actualZValue.Value, expectedValue);
        }

        [Theory]
        [InlineData("-xyz 'apply this to x, y and z'", "apply this to x, y and z")]
        [InlineData("-xyz salmon", "salmon")]
        [InlineData("-xyz 'salmon'", "salmon")]
        public void combined_short_options_should_have_the_same_value(string arguments, string expectedValue)
        {
            arguments = arguments.ReplaceWithDoubleQuotes();
            expectedValue = expectedValue.ReplaceWithDoubleQuotes();

            var sut = new Fclp.FluentCommandLineParser();

            string actualXValue = null;
            string actualYValue = null;
            string actualZValue = null;

            sut.Setup<string>('x').Callback(x => actualXValue = x);
            sut.Setup<string>('y').Callback(y => actualYValue = y);
            sut.Setup<string>('z').Callback(z => actualZValue = z);

            var args = arguments.ParseArguments();

            var results = sut.Parse(args);

            Assert.False(results.HasErrors);
            Assert.Equal(actualXValue, expectedValue);
            Assert.Equal(actualYValue, expectedValue);
            Assert.Equal(actualZValue, expectedValue);
        }
    }
}