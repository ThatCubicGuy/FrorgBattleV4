namespace FrogBattleV4.Core.Abilities.Components;

/// <summary>
/// A command component 
/// </summary>
public interface IShardCommand
{
    void Generate(ShardResolutionScope scope, BattleEnvironment env, LinkResolutionBuilder builder);
}