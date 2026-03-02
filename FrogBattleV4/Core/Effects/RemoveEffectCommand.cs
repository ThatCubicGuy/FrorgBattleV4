using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.AbilitySystem;

namespace FrogBattleV4.Core.Effects;

public record RemoveEffectCommand : IBattleCommand
{
    [NotNull] public required System.Func<StatusEffects.StatusEffectInstance, bool> Query { get; init; }
    [NotNull] public required IBattleMember Target { get; init; }
    [NotNull] public required System.Random Rng { get; init; }
    public IBattleMember Source { get; init; }
    public double RemovalChance { get; init; } = 1;
    public int RemovedStacks { get; init; } = 0;
    public int RemovedTurns { get; init; } = 0;
}