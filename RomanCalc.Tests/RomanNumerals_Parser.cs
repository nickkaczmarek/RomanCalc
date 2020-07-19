using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RomanCalc.Tests
{
    public class RomanNumerals_Parser
    {
        [Fact]
        public void should_parse_arabic_to_roman()
        {
            var expected = "CXXIII";
            var actual = RomanCalc.RomanNumerals.Parser.ArabicToRoman(123);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void should_parse_roman_to_arabic()
        {
            var expected = "123";
            var actual = RomanCalc.RomanNumerals.Parser.RomanToArabic("CXXIII");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void should_be_able_to_parse_file()
        {
            var expected = new List<string>() {
                "III", "29", "38", "CCXCI", "1999", "Cannot parse butt", "nulla"
            };
            var actual = RomanCalc.ConsoleHelper.HandleFile("test");
            var rnd = new Random().Next(0, expected.Count);
            Assert.Equal(expected[rnd], actual.ElementAt(rnd));
        }

        [Fact]
        public void should_be_able_to_parse_input()
        {
            var expected = "MMMCXCII";
            var actual = ConsoleHelper.HandleInput("3192");
            Assert.Equal(expected, actual);
        }
    }
}
