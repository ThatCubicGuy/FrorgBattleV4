using System.Collections.Immutable;

namespace FrogBattleV4.Core.Entities;

public class Fighter
{
    public required string Name { get; init; }
    public required ImmutableArray<ShardId> Shards { get; init; }
    // TODO (Nova): Modifiers, documentation?
}