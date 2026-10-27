using System.Collections.Immutable;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Modifiers.PassiveEffects;

namespace FrogBattleV4.Core.Entities;

public class Fighter
{
    public required string Name { get; init; }
    public required ImmutableArray<ShardId> Shards { get; init; }
    public ImmutableArray<PassiveModifier> PassiveModifiers { get; init; } = [];
    public ImmutableArray<Reaction> Reactions { get; init; } = [];
    public ImmutableDictionary<PoolName, Pool> Pools { get; init; } = ImmutableDictionary<PoolName, Pool>.Empty;
    public ImmutableDictionary<CounterName, Counter> Counters { get; init; } = ImmutableDictionary<CounterName, Counter>.Empty;
    // TODO (Nova): Modifiers, documentation?
}