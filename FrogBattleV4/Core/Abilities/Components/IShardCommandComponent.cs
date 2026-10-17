namespace FrogBattleV4.Core.Abilities.Components;

/// <summary>
/// A command component generates shard commands to the builder.
/// </summary>
public interface IShardCommandComponent
{
    void GenerateCommands(LinkResolutionState state, BattleEnvironment env, LinkResolutionBuilder builder);
}