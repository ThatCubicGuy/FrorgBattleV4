using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Modifiers.PassiveEffects.Conditions;
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PoolValueCondition), "pool_value")]
[JsonDerivedType(typeof(StatValueCondition), "stat_value")]
[JsonDerivedType(typeof(StatusEffectCondition), "status_effect")]
public interface IConditionComponent
{
    /// <summary>
    /// Gets the value that this condition is fulfilled by.
    /// This value may be negative.
    /// </summary>
    /// <param name="subject">The subject holding this passive effect.</param>
    /// <param name="reference">The reference to evaluate against.</param>
    /// <param name="env">Environment to resolve in.</param>
    /// <returns>The fulfillment value.</returns>
    [Pure]
    int GetContribution(EntityUid subject, EntityUid? reference, BattleEnvironment env);
}