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

            string result;

            if (File.Exists(param))
            {
                var fileContents = System.IO.File.ReadAllText(param).Split("\n");

                fileContents.ToList().ForEach(x =>
                {
                    result = int.TryParse(x, out int parsedArg)
                        ? RomanNumerals.Parser.ArabicToRoman(parsedArg)
                        : RomanNumerals.Parser.RomanToArabic(x).ToString();

                    Console.WriteLine(result);
                });
                return;
            }

            result = int.TryParse(param, out int parsedArg)
                ? RomanNumerals.Parser.ArabicToRoman(parsedArg)
                : RomanNumerals.Parser.RomanToArabic(param).ToString();


            Console.WriteLine(result);
        }
    }

    static class RomanNumerals
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

        static int[] Rungs = {
            1000,
            100,
            10,
            1,
        };

        public static class Parser
        {
            public static string RomanToArabic(string Roman) => RomanToArabicParser(Roman);
            public static string ArabicToRoman(int Arabic) => ArabicToRomanParser(Arabic);
            static string RomanToArabicParser(string Roman)
            {
                if (!Regex.Match(Roman, @"[IVXLCDM]").Success) return $"Cannot parse {Roman}";

                int total = 0;
                for (var i = 0; i < Roman.Length; i++)
                {
                    int current = RomanToArabicMapping[Roman[i].ToString()];
                    if (i > 0)
                    {
                        string currentChar = Roman[i].ToString();
                        string nextChar = i == Roman.Length - 1 ? "" : Roman[i + 1].ToString();

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
            static string ArabicToRomanParser(int Arabic, string total = "", int count = 0)
            {
                if (count > 1 && Arabic == 0) return total; 
                if (Arabic <= 0 || Arabic > 3999) return Arabic == 0 ? "nulla" : $"Cannot parse {Arabic}";

                var level = Rungs[count];
                var times = Arabic / level;
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
                                total += $"{modifier}{letter}";
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
                        var remainder = Arabic % level;
                        if (remainder <= 0) return total;
                        return ArabicToRomanParser(remainder, total, count + 1);
                    }
                }

                bool theEnd = Rungs.Length - 1 == count;
                if (times == 0 && !theEnd) return ArabicToRomanParser(Arabic, total, count + 1);
                
                if (!theEnd)
                {
                    var remainder = Arabic % level;
                    return ArabicToRomanParser(remainder, total, count + 1);
                }

                return total;
            }
        }
    }
}