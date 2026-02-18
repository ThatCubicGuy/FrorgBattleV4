using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core;

public static class StringExtensions
{
    public static string ToSnakeCase(this string input)
    {
        return string.Join('_', input.Words().Select(s => s.ToLower()));
    }

    public static string ToPascalCase(this string input)
    {
        return string.Concat(input.Words().Select(s => char.ToUpper(s[0]) + s[1..].ToLower()));
    }

    public static string ToScreamingSnakeCase(this string input)
    {
        return string.Join('_', input.Words().Select(s => s.ToUpper()));
    }

    /// <summary>
    /// Splits the given string into words as best it can.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static List<string> Words(this string input)
    {
        var result = new List<string>();
        // Check if the string is in all caps before splitting every letter
        var caps = !input.Any(char.IsLower);
        var allCaps = string.Equals(input, input.ToUpperInvariant());
        while (!string.IsNullOrWhiteSpace(input))
        {
            result.Add(NextWord(ref input, allCaps));
        }

        return result;
    }

    private static string NextWord(ref string input, bool allCaps)
    {
        // Take the next complete word
        var word = input[0] + string.Concat(input.Skip(1).TakeWhile(allCaps ? char.IsLetter : char.IsLower));

        // Skip that word and any non-letters in input
        input = string.Concat(input[word.Length..].SkipWhile(c => !char.IsLetter(c)));

        return word;
    }
}

public static class DoubleExtensions
{
    /// <summary>
    /// Determines whether the value given is contained within an inclusive interval.
    /// </summary>
    /// <remarks>
    /// Null is treated as a lack of limit - thus, any value
    /// is contained within an interval of null and null.
    /// </remarks>
    /// <param name="value">The value to check.</param>
    /// <param name="min">The inclusive lower bound of the interval.</param>
    /// <param name="max">The inclusive upper bound of the interval.</param>
    /// <returns>True if the given value is contained in the interval, false otherwise.</returns>
    public static bool IsWithinRange(this double value, double? min, double? max)
    {
        return (!min.HasValue || min.Value <= value) && (!max.HasValue || value <= max.Value);
    }

    /// <summary>
    /// Determines whether the value given is contained within an exclusive interval.
    /// </summary>
    /// <remarks>
    /// Null is treated as a lack of limit - thus, any value
    /// is contained within an interval of null and null.
    /// </remarks>
    /// <param name="value">The value to check.</param>
    /// <param name="min">The exclusive lower bound of the interval.</param>
    /// <param name="max">The exclusive upper bound of the interval.</param>
    /// <returns>True if the given value is contained in the interval, false otherwise.</returns>
    public static bool IsWithinRangeExclusive(this double value, double? min, double? max)
    {
        return (!min.HasValue || min.Value < value) && (!max.HasValue || value < max.Value);
    }
}