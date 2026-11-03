using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities.Reactions;

[JsonConverter(typeof(JsonStringEnumConverter<ReactionEntity>))]
public enum ReactionEntity
{
    [JsonStringEnumMemberName("this")]
    This,
    [JsonStringEnumMemberName("allies")]
    Allies,
    [JsonStringEnumMemberName("enemies")]
    Enemies,
}