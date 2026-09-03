namespace FrogBattleV4.Core.Abilities.Components;

/// <summary>
/// A command component 
/// </summary>
public interface IShardCommand
{
    void Generate(LinkResolutionState state, BattleEnvironment env, LinkResolutionBuilder builder);
}