#nullable enable
using System.ComponentModel;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;

namespace FrogBattleV4.Core.EffectSystem;

public struct EffectApplicationContext
{
    public ICharacter Source;
    public ISupportsEffects Target;
    public ActiveEffectDefinition Definition;
    public double ApplicationChance;
    public ChanceType ChanceType;
    public System.Random Rng;
    public uint InitialTurns;
    public uint InitialStacks;
}

public static class EffectApplicationContextExtensions
{
    public static bool CanApply(this EffectApplicationContext effect)
    {
        var totalChance = effect.ChanceType switch
        {
            ChanceType.Fixed => effect.ApplicationChance,
            ChanceType.Base => effect.ApplicationChance +
                               effect.Source.GetStat(nameof(Stat.EffectHitRate), effect.Target as IBattleMember) -
                               ((effect.Target as IBattleMember)?.GetStat(nameof(Stat.EffectRes), effect.Source) ?? 0),
            _ => throw new InvalidEnumArgumentException($"Invalid chance type: {effect.ChanceType}")
        };

        return effect.Rng.NextDouble() < totalChance;
    }
}

public enum ChanceType
{
    Fixed,
    Base
}