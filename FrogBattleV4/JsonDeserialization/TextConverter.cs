using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FrogBattleV4.Core;

namespace FrogBattleV4.JsonDeserialization;

public class TextConverter : JsonConverter<TextTranslatable>
{
    public override TextTranslatable? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, TextTranslatable value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, AppSerializerContext.Default.TextTranslatable);
    }
}