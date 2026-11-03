using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core;

/// <summary>
/// Predicate for an entity to be matched against.
/// </summary>
public class EntityPredicate : GamePredicate<Entity>
{
    public FighterId? Id { get; init; }
    public ShardId[]? Shards { get; init; }

    // TODO (Nova): Expand predicate

    public override bool Evaluate(Entity target)
    {
        return (Id?.Equals(target.Owner) ?? true);
    }
}