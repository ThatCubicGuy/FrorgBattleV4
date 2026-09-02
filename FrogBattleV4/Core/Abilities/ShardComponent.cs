namespace FrogBattleV4.Core.Abilities;

public abstract class ShardComponent(IShard parentShard)
{
    public IShard ParentShard { get; init; } = parentShard;
}