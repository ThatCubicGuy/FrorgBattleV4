using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core;

public static class GeneralPurposeExtensions
{
    public static string ToSnakeCase(this string input)
    {
        return string.Join('_', input.Words().Select(s => s.ToLower()));
    }

    public static string ToPascalCase(this string input)
    {
        return string.Concat(input.Words().Select(s => char.ToUpper(s[0]) + s[1..].ToLower()));
    }

    private static List<string> Words(this string input)
    {
        var result = new List<string>();
        // Check if the string is in all caps before splitting every letter
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