#nullable enable
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.EffectSystem.StatusEffects;

namespace FrogBattleV4.Core.EffectSystem;

public struct StatusEffectApplicationContext()
{
    [NotNull] public required StatusEffectDefinition Definition { get; init; }
    [NotNull] public ISupportsEffects Target { get; init; }
    public BattleMember? Source { get; init; }
    public double ApplicationChance { get; init; } = 1;
    public ChanceType ChanceType { get; init; } = ChanceType.Fixed;
    public int InitialStacks { get; init; } = 1;
    public required int InitialTurns { get; init; }
    [NotNull] public required System.Random Rng { get; init; }
}

public enum ChanceType
{
    Fixed,
    Base
}