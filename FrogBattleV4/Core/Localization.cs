using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text.Json;

namespace FrogBattleV4.Core;

public static class Localization
{
    private static Dictionary<string, string> _translationFile;
    
    /// <summary>
    /// Loads a JSON list of key-value pairs representing the translation dictionary for use in game.
    /// </summary>
    /// <param name="filePath">The path to a file containing a JSON dictionary.</param>
    /// <remarks>All exceptions pertaining to <see cref="File.ReadAllText(string)"/>
    /// and <see cref="JsonSerializer.Deserialize{TValue}(string, JsonSerializerOptions?)"/>
    /// apply here as well.</remarks>
    public static void LoadTranslationFile(string filePath)
    {
        var jsonText = File.ReadAllText(filePath);
        _translationFile = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonText);
    }

    /// <summary>
    /// Gets the localization corresponding to the given key in the currently loaded translation file.
    /// </summary>
    /// <param name="key">Key to search in the dictionary.</param>
    /// <returns>The translated key, or the key if it does not appear in the dictionary.</returns>
    /// <exception cref="InvalidOperationException">No translation file loaded.</exception>
    [Pure] public static string GetTranslation([NotNull] string key)
    {
        return _translationFile is null
            ? throw new InvalidOperationException("Translation file not loaded.")
            : _translationFile.GetValueOrDefault(key, key);
    }

    /// <summary>
    /// Gets the localization and formats it with the given arguments, in order.
    /// </summary>
    /// <param name="formatKey">Dictionary key for the localization.</param>
    /// <param name="args">Format arguments.</param>
    /// <returns>Formatted translation.</returns>
    /// <exception cref="InvalidOperationException">No translation file loaded.</exception>
    [Pure] public static string GetTranslationFormatted([NotNull] string formatKey, [NotNull] params object[] args)
    {
        return string.Format(GetTranslation(formatKey), args);
    }
}