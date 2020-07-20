using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RomanCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            string param = "";
            if (args != null && args.Length > 0) param = args[0];

            while (string.IsNullOrWhiteSpace(param))
            {
                Console.WriteLine("Please specify a filename as a parameter.");
                Console.WriteLine("Or provide a roman numeral or number as argument");
                param = Console.ReadLine();
            }

            if (File.Exists(param))
            {
                ConsoleHelper.HandleFile(param).ToList().ForEach(x => Console.WriteLine(x));
                return;
            }

            Console.WriteLine(ConsoleHelper.HandleInput(param));
        }
    }

    public struct ConsoleHelper
    {
        public static IEnumerable<string> HandleFile(string filePath) =>
         System.IO.File.ReadAllText(filePath).Split("\n").ToList().Select(x => int.TryParse(x, out int parsedArg)
                    ? RomanNumerals.Parser.ArabicToRoman(parsedArg)
                    : RomanNumerals.Parser.RomanToArabic(x).ToString());

        public static string HandleInput(string input) => int.TryParse(input, out int parsedArg)
                ? RomanNumerals.Parser.ArabicToRoman(parsedArg)
                : RomanNumerals.Parser.RomanToArabic(input).ToString();
    }

    public static class RomanNumerals
    {
        static Dictionary<string, int> RomanToArabicMapping = new Dictionary<string, int>() {
            {"I", 1},
            {"IV", 4},
            {"V", 5},
            {"IX", 9},
            {"X", 10},
            {"XL", 40},
            {"L", 50},
            {"XC", 90},
            {"C", 100},
            {"CD", 400},
            {"D", 500},
            {"CM", 900},
            {"M", 1000}
        };
        static int[] Rungs = { 1000, 100, 10, 1 };
        public static class Parser
        {
            public static string RomanToArabic(string roman) => RomanToArabicParser(roman);
            public static string ArabicToRoman(int arabic) => ArabicToRomanParser(arabic);
            static string RomanToArabicParser(string roman)
            {
                if (!Regex.Match(roman, @"[IVXLCDM]").Success) return $"Cannot parse {roman}";

                int total = 0;
                for (var i = 0; i < roman.Length; i++)
                {
                    int current = RomanToArabicMapping[roman[i].ToString()];
                    if (i > 0)
                    {
                        string currentChar = roman[i].ToString();
                        string nextChar = i == roman.Length - 1 ? "" : roman[i + 1].ToString();

                        if (RomanToArabicMapping.ContainsKey(currentChar + nextChar))
                        {
                            total += RomanToArabicMapping[currentChar + nextChar];
                            i++;
                        }
                        else
                        {
                            total += current;
                        }
                    }
                    else
                    {
                        total += current;
                    }
                }
                return total.ToString();
            }
            static string ArabicToRomanParser(int arabic, string total = "", int count = 0)
            {
                if (count > 1 && arabic == 0) return total;
                if (arabic <= 0 || arabic > 3999) return arabic == 0 ? "nulla" : $"Cannot parse {arabic}";

                var level = Rungs[count];
                var times = arabic / level;
                var modifier = level >= 100 ? "C" : level >= 10 ? "X" : "I";
                var halfWay = level >= 100 ? "D" : level >= 10 ? "L" : "V";
                var letter = RomanToArabicMapping.First(x => x.Value == level).Key;

                if (times >= 1)
                {
                    if (count > 0)
                    {
                        switch (times)
                        {
                            case int t when t < 4:
                                total += string.Concat(Enumerable.Repeat(letter, times));
                                break;
                            case int t when t == 4:
                                total += $"{modifier}{halfWay}";
                                break;
                            case int t when t == 5:
                                total += halfWay;
                                break;
                            case int t when t < 9:
                                total += $"{halfWay}{string.Concat(Enumerable.Repeat(letter, times - 5))}";
                                break;
                            case int t when t == 9:
                                letter = RomanToArabicMapping.First(x => x.Value == Rungs[count - 1]).Key;
                                total += $"{modifier}{letter}";
                                break;
                        }
                    }
                    else
                    {
                        total += string.Concat(Enumerable.Repeat(letter, times));
                        var remainder = arabic % level;
                        if (remainder <= 0) return total;
                        return ArabicToRomanParser(remainder, total, count + 1);
                    }
                }

                bool theEnd = Rungs.Length - 1 == count;
                if (times == 0 && !theEnd) return ArabicToRomanParser(arabic, total, count + 1);

                if (!theEnd)
                {
                    var remainder = arabic % level;
                    return ArabicToRomanParser(remainder, total, count + 1);
                }

                return total;
            }
        }
    }
}
