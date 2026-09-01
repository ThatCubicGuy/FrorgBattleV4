namespace FrogBattleV4.Core.Abilities.Components;

/// <summary>
/// A command component 
/// </summary>
public interface IShardCommandComponent
{
    void Resolve(ShardLinkContext context, LinkResolutionBuilder builder);
}

public interface IShardAction
{
    void Apply();
}