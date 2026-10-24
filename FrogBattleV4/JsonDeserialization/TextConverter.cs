using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FrogBattleV4.Core;
using FrogBattleV4.JsonDeserialization.DataTransferObjects;

namespace FrogBattleV4.JsonDeserialization;

internal class TextConverter : JsonConverter<TextTranslatable>
{
    public override TextTranslatable? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dto = reader.TokenType switch
        {
            JsonTokenType.StartObject => JsonSerializer.Deserialize<TextDto>(ref reader, options),
            JsonTokenType.StartArray => throw new NotImplementedException(),
            JsonTokenType.String => new TextDto { Text = reader.GetString() },
            _ => null
        } ?? throw new JsonException("Expected one of array, text object, or string.");

        if (dto.Translate is not null)
        {
            
        }
        
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, TextTranslatable value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, AppSerializerContext.Default.TextTranslatable);
    }
}