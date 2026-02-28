#nullable enable
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.CharacterSystem;

/// <summary>
/// Standard positive pool component for a character.
/// </summary>
public class CharacterPoolDefinition : IPoolDefinition
{
    private readonly HashSet<PoolTag> _tags = [];

    public required PoolId Id { get; init; }
    public double InitialPercent { get; init; } = 0;
    double IPoolDefinition.GetInitialValue(ModifierContext ctx) => InitialPercent * ctx.ComputeStat(MaxValueStat);
    public required StatId MaxValueStat { get; init; }
    public IEnumerable<PoolTag> Tags
    {
        get => _tags;
        init => _tags = value.ToHashSet();
    }

    public double? GetMaxValue(ModifierContext ctx) => new PoolValueQuery
    {
        Channel = PoolValueChannel.Max,
        PoolId = Id
    }.Compute(ctx.ComputeStat(MaxValueStat), ctx);

    public double? GetMinValue(ModifierContext ctx) => new PoolValueQuery
    {
        Channel = PoolValueChannel.Max,
        PoolId = Id
    }.Compute(0, ctx);
}