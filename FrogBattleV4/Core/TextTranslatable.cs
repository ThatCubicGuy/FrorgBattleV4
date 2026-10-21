using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core;

[JsonConverter(typeof(TextConverter))]
public class TextTranslatable
{
    public required string Text { get; init; }
    public bool Translatable { get; init; } = true;
    public string[]? Parameters { get; init; }
}