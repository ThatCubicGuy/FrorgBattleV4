using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core;

/// <summary>
/// Predicate for an entity to be matched against.
/// </summary>
public class EntityPredicate
{
    public FighterId? Id { get; init; }
    public ShardId[]? Shards { get; init; }

    // TODO (Nova): Expand predicate

    public bool Matches(Entity target)
    {
        return (Id?.Equals(target.Owner) ?? true);
    }
}