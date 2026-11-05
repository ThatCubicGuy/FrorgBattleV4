using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core;

/// <summary>
/// Predicate for an entity to be matched against.
/// </summary>
public class EntityPredicate : IGamePredicate<EntityInstance>
{
    public FighterId? Id { get; init; }
    public ShardId[]? Shards { get; init; }

    // TODO (Nova): Expand predicate

    public virtual bool Evaluate(EntityInstance target)
    {
        return (Id?.Equals(target.Owner) ?? true);
    }
}