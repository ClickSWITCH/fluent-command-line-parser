#region License
// CommandLineParserEngineMark2TestsXUnit.cs
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

using System.Linq;
using Fclp.Internals.Extensions;
using Fclp.Internals.Parsing;
using Xunit;

namespace Fclp.Tests.Internals
{

    public class CommandLineParserEngineMark2TestsXUnit
    {
        [Theory]
        [InlineData("-f", "-", "f", null)]
        [InlineData("/f", "/", "f", null)]
        [InlineData("--f", "--", "f", null)]
        [InlineData("-f apple", "-", "f", "apple")]
        [InlineData("/f apple", "/", "f", "apple")]
        [InlineData("--f apple", "--", "f", "apple")]
        [InlineData("/fruit", "/", "fruit", null)]
        [InlineData("--fruit", "--", "fruit", null)]
        [InlineData("/fruit apple", "/", "fruit", "apple")]
        [InlineData("--fruit apple", "--", "fruit", "apple")]
        [InlineData("/fruit:apple", "/", "fruit", "apple")]
        [InlineData("--fruit:apple", "--", "fruit", "apple")]
        [InlineData("-f:apple", "-", "f", "apple")]
        [InlineData("/fruit=apple", "/", "fruit", "apple")]
        [InlineData("--fruit=apple", "--", "fruit", "apple")]
        [InlineData("-f=apple", "-", "f", "apple")]
        [InlineData("/fruit 'apple pear plum'", "/", "fruit", "'apple pear plum'")]
        [InlineData("--fruit 'apple pear plum'", "--", "fruit", "'apple pear plum'")]
        [InlineData("-f 'apple pear plum'", "-", "f", "'apple pear plum'")]
        [InlineData("/fruit:'apple pear plum'", "/", "fruit", "'apple pear plum'")]
        [InlineData("--fruit:'apple pear plum'", "--", "fruit", "'apple pear plum'")]
        [InlineData("-f:'apple pear plum'", "-", "f", "'apple pear plum'")]
        [InlineData("/fruit='apple pear plum'", "/", "fruit", "'apple pear plum'")]
        [InlineData("--fruit='apple pear plum'", "--", "fruit", "'apple pear plum'")]
        [InlineData("-f='apple pear plum'", "-", "f", "'apple pear plum'")]
        public void should_parse_single_options_correctly(
            string arguments,
            string expectedPrefix,
            string expectedKey,
            string expectedValue)
        {
            var convertedArgs = arguments.ParseArguments();
            expectedValue = expectedValue.ReplaceWithDoubleQuotes();

            var sut = new CommandLineParserEngineMark2();
            var result = sut.Parse(convertedArgs);

            Assert.Single(result.ParsedOptions);
            Assert.Empty(result.AdditionalValues);

            var actualParsedOption = result.ParsedOptions.First();

            Assert.Equal(expectedKey, actualParsedOption.Key);
            Assert.Equal(expectedValue, actualParsedOption.Value);
            Assert.Equal(expectedPrefix, actualParsedOption.Prefix);
        }

        [Theory]
        [InlineData("-f -v", "-", "f", null, "-", "v", null)]
        [InlineData("/f /v", "/", "f", null, "/", "v", null)]
        [InlineData("--f --v", "--", "f", null, "--", "v", null)]
        [InlineData("-f apple -v onion", "-", "f", "apple", "-", "v", "onion")]
        [InlineData("/f apple /v onion", "/", "f", "apple", "/", "v", "onion")]
        [InlineData("--f apple --v onion", "--", "f", "apple", "--", "v", "onion")]
        [InlineData("/fruit /vegetable", "/", "fruit", null, "/", "vegetable", null)]
        [InlineData("--fruit --vegetable", "--", "fruit", null, "--", "vegetable", null)]
        [InlineData("/fruit apple /vegetable onion", "/", "fruit", "apple", "/", "vegetable", "onion")]
        [InlineData("--fruit apple --vegetable onion", "--", "fruit", "apple", "--", "vegetable", "onion")]
        [InlineData("/fruit:apple /vegetable:onion", "/", "fruit", "apple", "/", "vegetable", "onion")]
        [InlineData("--fruit:apple --vegetable:onion", "--", "fruit", "apple", "--", "vegetable", "onion")]
        [InlineData("-f:apple -v: onion", "-", "f", "apple", "-", "v", "onion")]
        [InlineData("/fruit=apple /vegetable=onion", "/", "fruit", "apple", "/", "vegetable", "onion")]
        [InlineData("--fruit=apple --vegetable=onion", "--", "fruit", "apple", "--", "vegetable", "onion")]
        [InlineData("-f=apple -v=onion", "-", "f", "apple", "-", "v", "onion")]
        [InlineData("/fruit 'apple pear plum' /vegetable 'onion carrot peas'", "/", "fruit", "'apple pear plum'", "/", "vegetable", "'onion carrot peas'")]
        [InlineData("--fruit 'apple pear plum' --vegetable 'onion carrot peas'", "--", "fruit", "'apple pear plum'", "--", "vegetable", "'onion carrot peas'")]
        [InlineData("-f 'apple pear plum' -v 'onion carrot peas'", "-", "f", "'apple pear plum'", "-", "v", "'onion carrot peas'")]
        [InlineData("/fruit:'apple pear plum' /vegetable:'onion carrot peas'", "/", "fruit", "'apple pear plum'", "/", "vegetable", "'onion carrot peas'")]
        [InlineData("--fruit:'apple pear plum' --vegetable:'onion carrot peas'", "--", "fruit", "'apple pear plum'", "--", "vegetable", "'onion carrot peas'")]
        [InlineData("-f:'apple pear plum' -v:'onion carrot peas'", "-", "f", "'apple pear plum'", "-", "v", "'onion carrot peas'")]
        [InlineData("/fruit='apple pear plum' /vegetable='onion carrot peas'", "/", "fruit", "'apple pear plum'", "/", "vegetable", "'onion carrot peas'")]
        [InlineData("--fruit='apple pear plum' --vegetable='onion carrot peas'", "--", "fruit", "'apple pear plum'", "--", "vegetable", "'onion carrot peas'")]
        [InlineData("-f='apple pear plum' -v='onion carrot peas'", "-", "f", "'apple pear plum'", "-", "v", "'onion carrot peas'")]
        public void should_parse_double_options_correctly(
            string arguments,
            string firstExpectedKeyChar,
            string firstExpectedKey,
            string firstExpectedValue,
            string secondExpectedKeyChar,
            string secondExpectedKey,
            string secondExpectedValue)
        {
            var convertedArgs = arguments.ParseArguments();

            firstExpectedValue = firstExpectedValue.ReplaceWithDoubleQuotes();
            secondExpectedValue = secondExpectedValue.ReplaceWithDoubleQuotes();

            var sut = new CommandLineParserEngineMark2();
            var result = sut.Parse(convertedArgs);

            Assert.Equal(2, result.ParsedOptions.Count());
            Assert.Empty(result.AdditionalValues);

            var first = result.ParsedOptions.First();

            Assert.Equal(firstExpectedKey, first.Key);
            Assert.Equal(firstExpectedValue, first.Value);
            Assert.Equal(firstExpectedKeyChar, first.Prefix);

            var second = result.ParsedOptions.ElementAt(1);

            Assert.Equal(secondExpectedKey, second.Key);
            Assert.Equal(secondExpectedValue, second.Value);
            Assert.Equal(secondExpectedKeyChar, second.Prefix);
        }

        [Theory]
        [InlineData("-b", "-", "b", null, null)]
        [InlineData("-b+", "-", "b", null, "+")]
        [InlineData("-b-", "-", "b", null, "-")]
        [InlineData("/b", "/", "b", null, null)]
        [InlineData("/b+", "/", "b", null, "+")]
        [InlineData("/b-", "/", "b", null, "-")]
        [InlineData("--b", "--", "b", null, null)]
        [InlineData("--b+", "--", "b", null, "+")]
        [InlineData("--b-", "--", "b", null, "-")]
        public void should_parse_boolean_values_correctly(
            string arguments,
            string expectedPrefix,
            string expectedKey,
            string expectedValue,
            string expectedSuffix)
        {
            var convertedArgs = arguments.ParseArguments();

            var sut = new CommandLineParserEngineMark2();
            var result = sut.Parse(convertedArgs);

            Assert.Single(result.ParsedOptions);
            Assert.Empty(result.AdditionalValues);

            var actualParsedOption = result.ParsedOptions.First();

            Assert.Equal(expectedKey, actualParsedOption.Key);
            Assert.Equal(expectedValue, actualParsedOption.Value);
            Assert.Equal(expectedPrefix, actualParsedOption.Prefix);
            Assert.Equal(expectedSuffix, actualParsedOption.Suffix);
        }

        [Theory]
        [InlineData("-xyz", "-", null, null, "x", "y", "z")]
        [InlineData("-xyz+", "-", "+", null, "x", "y", "z")]
        [InlineData("-xyz-", "-", "-", null, "x", "y", "z")]
        public void should_parse_combined_boolean_values_correctly(
            string arguments,
            string expectedPrefix,
            string expectedSuffix,
            string expectedValue,
            string firstExpectedKey,
            string secondExpectedKey,
            string thirdExpectedKey)
        {
            var convertedArgs = arguments.ParseArguments();

            expectedValue = expectedValue.ReplaceWithDoubleQuotes();

            var sut = new CommandLineParserEngineMark2();
            var result = sut.Parse(convertedArgs);

            Assert.Equal(3, result.ParsedOptions.Count());
            Assert.Empty(result.AdditionalValues);

            var first = result.ParsedOptions.First();

            Assert.Equal(firstExpectedKey, first.Key);
            Assert.Equal(expectedValue, first.Value);
            Assert.Equal(expectedPrefix, first.Prefix);
            Assert.Equal(expectedSuffix, first.Suffix);

            var second = result.ParsedOptions.ElementAt(1);

            Assert.Equal(secondExpectedKey, second.Key);
            Assert.Equal(expectedValue, second.Value);
            Assert.Equal(expectedPrefix, second.Prefix);
            Assert.Equal(expectedSuffix, second.Suffix);

            var third = result.ParsedOptions.ElementAt(2);

            Assert.Equal(thirdExpectedKey, third.Key);
            Assert.Equal(expectedValue, third.Value);
            Assert.Equal(expectedPrefix, third.Prefix);
            Assert.Equal(expectedSuffix, third.Suffix);
        }
    }
}
