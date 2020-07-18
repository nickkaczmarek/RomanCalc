using System;
using System.Collections.Generic;
using System.Linq;

namespace RomanCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Please specify a filename as a parameter.");
                return;
            }

            var fileContents = System.IO.File.ReadAllText(args[0]).Split("\n");

            fileContents.ToList().ForEach(x => {
                int.TryParse(x, out int result);
                if (result == 0) {
                    Console.WriteLine(RomanNumerals.Parser.RomanToArabic(x));
                } else {
                    Console.WriteLine(RomanNumerals.Parser.ArabicToRoman(result));
                }
            });
        }
    }

    static class RomanNumerals
    {
        static Dictionary<string, int> Mapping = new Dictionary<string, int>() {
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

        static int[] Stacking = {
            1000,
            100,
            10,
            1,
        };

        public static class Parser
        {
            public static int RomanToArabic(string Roman) => RomanToArabicParser(Roman);
            public static string ArabicToRoman(int Arabic) => ArabicToRomanParser(Arabic);
            static int RomanToArabicParser(string Roman)
            {
                int total = 0;
                for (var i = 0; i < Roman.Length; i++)
                {
                    int current = Mapping[Roman[i].ToString()];
                    if (i > 0)
                    {
                        string currentChar = Roman[i].ToString();
                        string nextChar = i == Roman.Length - 1 ? "" : Roman[i + 1].ToString();

                        if (Mapping.ContainsKey(currentChar + nextChar))
                        {
                            total += Mapping[currentChar + nextChar];
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
                return total;
            }

            static string ArabicToRomanParser(int Arabic, string total = "", int count = 0)
            {
                if (count > 1 && Arabic == 0) { return total; }
                if (Arabic <= 0 || Arabic > 3999) { return "Cannot parse"; }

                var times = Arabic / Stacking[count];
                var modifier = Stacking[count] >= 100 ? "C" : Stacking[count] >= 10 ? "X" : "I";
                var halfWay = Stacking[count] >= 100 ? "D" : Stacking[count] >= 10 ? "L" : "V";
                var letter = Mapping.First(x => x.Value == Stacking[count]).Key;

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
                                letter = Mapping.First(x => x.Value == Stacking[count - 1]).Key;
                                total += $"{modifier}{letter}";
                                break;
                        }
                    }
                    else
                    {
                        total += string.Concat(Enumerable.Repeat(letter, times));
                        var remainder = Arabic % Stacking[count];
                        if (remainder <= 0) return total;
                        return ArabicToRomanParser(remainder, total, count + 1);
                    }
                }

                bool theEnd = Stacking.Length - 1 == count;
                if (times == 0 && !theEnd)
                {
                    return ArabicToRomanParser(Arabic, total, count + 1);
                }
                if (!theEnd)
                {
                    var remainder = Arabic % Stacking[count];
                    return ArabicToRomanParser(remainder, total, count + 1);
                }

                return total;
            }
        }
    }
}