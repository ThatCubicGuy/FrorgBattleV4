using System.Text.Json.Serialization;

namespace FrogBattleV4.JsonDeserialization;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(Core.TextTranslatable))]
[JsonSerializable(typeof(Core.Entities.Fighter))]
[JsonSerializable(typeof(Core.Abilities.Shard))]
[JsonSerializable(typeof(Core.Modifiers.StatusEffects.StatusEffect))]
internal partial class AppSerializerContext : JsonSerializerContext;