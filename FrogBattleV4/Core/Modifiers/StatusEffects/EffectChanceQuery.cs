using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public record EffectChanceQuery : DynamicQuery
{
    public required ApplyEffectData Data { get; init; }
}