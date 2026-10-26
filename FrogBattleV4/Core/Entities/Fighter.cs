using System.Collections.Immutable;
using FrogBattleV4.Core.Modifiers.PassiveEffects;

namespace FrogBattleV4.Core.Entities;

public class Fighter
{
    public required string Name { get; init; }
    public required ImmutableArray<ShardId> Shards { get; init; }
    public ImmutableArray<PassiveModifier> PassiveModifiers { get; init; } = [];
    // TODO (Nova): Modifiers, documentation?
}