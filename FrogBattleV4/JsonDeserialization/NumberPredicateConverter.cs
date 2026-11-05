using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.JsonDeserialization.DataTransferObjects;

namespace FrogBattleV4.JsonDeserialization;

public class NumberPredicateConverter : JsonConverter<NumberPredicate>
{
    public override NumberPredicate? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.Number => new NumberPredicate(reader.GetInt32()),
            JsonTokenType.StartObject => JsonSerializer.Deserialize(ref reader,
                    DtoSerializerContext.Default.NumberPredicateDto)
                ?.ToNumberPredicate(),
            _ => throw new JsonException("Expected a number or an object."),
        };
    }

    public override void Write(Utf8JsonWriter writer, NumberPredicate value, JsonSerializerOptions options)
    {
        if (value.Min == value.Max) writer.WriteNumberValue(value.Min!.Value);
        else
        {
            JsonSerializer.Serialize(writer, new NumberPredicateDto
            {
                Min = value.Min,
                Max = value.Max,
            }, DtoSerializerContext.Default.NumberPredicateDto);
        }
    }
}