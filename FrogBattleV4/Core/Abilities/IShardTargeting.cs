using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Targeting.AreaOfEffectTargeting), "area_of_effect")]
[JsonDerivedType(typeof(Targeting.BlastTargeting), "blast")]
[JsonDerivedType(typeof(Targeting.BounceTargeting), "bounce")]
[JsonDerivedType(typeof(Targeting.FilterTargeting), "filter")]
public interface IShardTargeting
{
    TargetingInstruction GenerateInstruction(ShardTargetingContext targeting, BattleEnvironment env);
}