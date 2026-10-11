namespace FrogBattleV4.Core.Abilities.Components;

/// <summary>
/// A command component generates shard actions to the builder.
/// </summary>
public interface IShardCommand
{
    void GenerateActions(LinkResolutionState state, BattleEnvironment env, LinkResolutionBuilder builder);
}