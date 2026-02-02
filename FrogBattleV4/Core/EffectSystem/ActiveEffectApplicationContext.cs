#nullable enable
using System.ComponentModel;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;

namespace FrogBattleV4.Core.EffectSystem;

public struct ActiveEffectApplicationContext()
{
    public Character Source { get; init; }
    public ISupportsEffects Target { get; init; }
    public required ActiveEffectDefinition Definition { get; init; }
    public double ApplicationChance { get; init; } = 1;
    public ChanceType ChanceType { get; init; } = ChanceType.Fixed;
    public required uint InitialTurns { get; init; }
    public uint InitialStacks { get; init; } = 1;
    public required System.Random Rng { get; init; }
}

public struct ActiveEffectRemovalContext()
{
    public Character Source { get; init; }
    public ISupportsEffects Target { get; init; }
    public required ActiveEffectDefinition Definition { get; init; }
    public double RemovalChance { get; init; } = 1;
    public required System.Random Rng { get; init; }
} 

public enum ChanceType
{
    Fixed,
    Base
}

public static class EffectApplicationContextExtensions
{
    public static bool CanApply(this ActiveEffectApplicationContext activeEffect)
    {
        var totalChance = activeEffect.ChanceType switch
        {
            ChanceType.Fixed => activeEffect.ApplicationChance,
            ChanceType.Base => activeEffect.ApplicationChance +
                               activeEffect.Source.GetStat(nameof(Stat.EffectHitRate), activeEffect.Target as BattleMember) -
                               ((activeEffect.Target as BattleMember)?.GetStat(nameof(Stat.EffectRes), activeEffect.Source) ?? 0),
            _ => throw new InvalidEnumArgumentException($"Invalid chance type: {activeEffect.ChanceType}")
        };

        return activeEffect.Rng.NextDouble() < totalChance;
    }
}