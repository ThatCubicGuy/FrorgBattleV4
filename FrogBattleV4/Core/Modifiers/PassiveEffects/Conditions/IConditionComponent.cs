using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Modifiers.PassiveEffects.Conditions;

public interface IConditionComponent
{
    /// <summary>
    /// Gets the value that this condition is fulfilled by.
    /// This value may be negative.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="reference"></param>
    /// <param name="env"></param>
    /// <returns>The fulfillment value.</returns>
    [Pure]
    int GetContribution(EntityUid subject, EntityUid? reference, BattleEnvironment env);
}