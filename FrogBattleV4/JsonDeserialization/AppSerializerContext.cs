using System.Text.Json.Serialization;

namespace FrogBattleV4.JsonDeserialization;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(Core.Abilities.Shard))]
[JsonSerializable(typeof(Core.TextTranslatable))]
internal partial class AppSerializerContext : JsonSerializerContext;