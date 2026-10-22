using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities;

/// <summary>
/// A command component generates shard commands to the builder.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Components.AttackComponent), "attack")]
[JsonDerivedType(typeof(Components.EffectComponent), "effect")]
public interface IShardCommandComponent
{
    void GenerateCommands(LinkResolutionState state, BattleEnvironment env, CommandBuilder builder);
}