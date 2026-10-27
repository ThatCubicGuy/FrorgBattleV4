using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace FrogBattleV4.JsonDeserialization;

/// <summary>
/// Converts a string token to a wrapper struct for data-driven enums. Accepts only lowercase alphanumeric characters.
/// </summary>
/// <typeparam name="TName">Type of the expandable enum to convert to.</typeparam>
internal partial class ExpandableEnumConverter<TName> : JsonConverter<TName> where TName : struct, IExpandableEnum
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
            Identifier = result
        };
    }

    public override TName ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException("Expected a property name.");
        }
        
        var result = reader.GetString()!.Normalize();
        
        if (!AlphanumericRegex().IsMatch(result))
        {
            throw new JsonException("Expected a string containing only lowercase alphanumeric characters.");
        }

        return new TName
        {
            Identifier = result
        };
    }

    public override void Write(Utf8JsonWriter writer, TName value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Identifier);
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, TName value, JsonSerializerOptions options)
    {
        writer.WritePropertyName(value.Identifier);
    }

    [GeneratedRegex("^[a-z][a-z0-9_]*$")]
    private static partial Regex AlphanumericRegex();
}

internal interface IExpandableEnum
{
    string Identifier { get; init; }
}