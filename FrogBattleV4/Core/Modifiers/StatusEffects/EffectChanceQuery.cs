using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public class EffectChanceQuery : CommandQuery
{
    public required ApplyEffectData Data { get; init; }
}