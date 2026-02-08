#nullable enable
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.EffectSystem.PassiveEffects.Conditions;

namespace FrogBattleV4.Core.EffectSystem;

public struct StatusEffectRemovalContext()
{
    public required StatusEffectQuery Query { get; init; }
    public BattleMember? Source { get; init; }
    [NotNull] public required ISupportsEffects Target { get; init; }
    public double RemovalChance { get; init; } = 1;
    public int RemovedStacks { get; init; } = 1;
    public int RemovedTurns { get; init; } = 0;
    [NotNull] public required System.Random Rng { get; init; }
}