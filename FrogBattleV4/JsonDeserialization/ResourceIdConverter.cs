using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FrogBattleV4.Core;

namespace FrogBattleV4.JsonDeserialization;

internal class ResourceIdConverter<TResourceId> : JsonConverter<TResourceId> where TResourceId : struct, IResourceId
{
    public override TResourceId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected a string.");
        }

        var value = reader.GetString()!.Normalize().ToLowerInvariant();

        var id = value.Contains(':')
            ? new ResourceId
            {
                Namespace = value.Split(':')[0],
                Path = value.Split(':')[1]
            }
            // If the identifier has no namespace, assume default.
            : new ResourceId
            {
                Namespace = "frorgbattle",
                Path = value
            };

        return new TResourceId
        {
            Value = id
        };
    }

    public override void Write(Utf8JsonWriter writer, TResourceId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value.ToString());
    }
}

public interface IResourceId
{
    ResourceId Value { get; init; }
}