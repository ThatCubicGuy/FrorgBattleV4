using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core;

[JsonConverter(typeof(TextConverter))]
public class TextTranslatable(string text)
{
    // TODO (Nova): HEAVILY OUTDATED !!! CHECK SCHEMA
    public string Text { get; init; } = text;
    public bool Translatable { get; init; } = true;
    public string[]? Parameters { get; init; }
}