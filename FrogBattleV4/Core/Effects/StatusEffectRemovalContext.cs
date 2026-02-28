using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.Effects;

public struct StatusEffectRemovalContext()
{
    #nullable enable
    public IBattleMember? Source { get; init; }
    #nullable disable
    public required System.Func<StatusEffects.StatusEffectInstance, bool> Query { get; init; }
    [NotNull] public required IBattleMember Target { get; init; }
    public double RemovalChance { get; init; } = 1;
    public int RemovedStacks { get; init; } = 0;
    public int RemovedTurns { get; init; } = 0;
    [NotNull] public required System.Random Rng { get; init; }
}