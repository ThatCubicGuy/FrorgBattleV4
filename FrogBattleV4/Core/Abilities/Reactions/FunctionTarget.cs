using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities.Reactions;

[JsonConverter(typeof(JsonStringEnumConverter<FunctionTarget>))]
public enum FunctionTarget
{
    [JsonStringEnumMemberName("this")]
    This,
    [JsonStringEnumMemberName("sender")]
    Sender,
    [JsonStringEnumMemberName("target")]
    Target,
    [JsonStringEnumMemberName("allies")]
    Allies,
    [JsonStringEnumMemberName("enemies")]
    Enemies,
}