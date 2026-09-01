namespace FrogBattleV4.Core.Abilities;

public abstract class ShardComponent(AbilityShard shard)
{
    public AbilityShard ParentShard { get; init; } = shard;
}