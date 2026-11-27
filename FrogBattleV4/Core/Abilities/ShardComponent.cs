using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities;

/// <summary>
/// Defines the base class for an object that generates shard commands to the builder.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Components.AttackComponent), "attack")]
[JsonDerivedType(typeof(Components.DebuffComponent), "debuff")]
[JsonDerivedType(typeof(Components.HealComponent), "heal")]
public abstract class ShardComponent
{
    public abstract void GenerateCommands(ShardResolutionScope scope, BattleEnvironment env, CommandBuilder builder);
}