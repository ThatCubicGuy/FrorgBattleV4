using System.Text.Json.Serialization;

namespace FrogBattleV4.JsonDeserialization.DataTransferObjects;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(NumberPredicateDto))]
public partial class DtoSerializerContext : JsonSerializerContext;