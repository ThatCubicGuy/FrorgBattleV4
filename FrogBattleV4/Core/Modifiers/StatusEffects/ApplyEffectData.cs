using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public record ApplyEffectData(
    StatusEffectId Effect,
    [property: JsonPropertyName("turns")]
    int InitialTurns,
    [property: JsonPropertyName("stacks")]
    int AddedStacks = 1);