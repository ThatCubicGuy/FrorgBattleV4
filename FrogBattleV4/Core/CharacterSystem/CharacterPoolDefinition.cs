#nullable enable
using System.Collections.Frozen;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.CharacterSystem;

/// <summary>
/// Standard positive pool component for a character.
/// </summary>
public class CharacterPoolDefinition : IPoolDefinition
{
    private readonly FrozenSet<PoolTag> _tags = [];

    public required PoolId Id { get; init; }
    public double InitialPercent { get; init; }
    double IPoolDefinition.GetInitialValue(ModifierContext ctx) => InitialPercent * ctx.ComputeStat(MaxValueStat);
    public required StatId MaxValueStat { get; init; }
    public FrozenSet<PoolTag> Tags
    {
        get => _tags;
        init => _tags = value.ToFrozenSet();
    }

    public double MaxValue => new PoolValueQuery
    {
        Channel = PoolValueChannel.Max,
        PoolId = Id,
        Ctx = new ModifierContext()
    }.Compute(new ModifierContext().ComputeStat(MaxValueStat));

    public double MinValue => new PoolValueQuery
    {
        Channel = PoolValueChannel.Max,
        PoolId = Id,
        Ctx = new ModifierContext()
    }.Compute(0);
}