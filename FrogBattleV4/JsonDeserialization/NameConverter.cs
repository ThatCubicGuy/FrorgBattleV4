using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.JsonDeserialization;

internal class NameConverter<TName> : JsonConverter<TName> where TName : struct, IPropertyName
{
    public override TName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected a string.");
        }

        return new TName
        {
            Name = reader.GetString()!.Normalize()
        };
    }

    public override void Write(Utf8JsonWriter writer, TName value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Name);
    }
}