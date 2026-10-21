using System.Text.Json.Serialization;

namespace FrogBattleV4.JsonDeserialization;

[JsonSerializable(typeof(Core.Abilities.Shard))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
public partial class AppSerializerContext : JsonSerializerContext;