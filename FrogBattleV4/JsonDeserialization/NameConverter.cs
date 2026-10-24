using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace FrogBattleV4.JsonDeserialization;

/// <summary>
/// Converts a string token to a StatName or PoolName. Accepts only lowercase alphanumeric characters.
/// </summary>
/// <typeparam name="TName"></typeparam>
internal partial class NameConverter<TName> : JsonConverter<TName> where TName : struct, IPropertyName
{
    public override TName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected a string.");
        }

        var result = reader.GetString()!.Normalize();

        if (!AlphanumericRegex().IsMatch(result))
        {
            throw new JsonException("Expected a string containing only lowercase alphanumeric characters.");
        }

        return new TName
        {
            Name = result
        };
    }

    public override void Write(Utf8JsonWriter writer, TName value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Name);
    }

    [GeneratedRegex("^[a-z][a-z0-9_]*$")]
    private static partial Regex AlphanumericRegex();
}

internal interface IPropertyName
{
    string Name { get; init; }
}