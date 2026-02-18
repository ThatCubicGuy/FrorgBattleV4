#nullable enable
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects.Conditions;

public interface IConditionComponent
{
    /// <summary>
    /// Gets the value that this condition is fulfilled by.
    /// This value may be negative.
    /// </summary>
    /// <param name="ctx">The context for which to check the contribution.</param>
    /// <returns>The fulfillment value.</returns>
    [Pure]
    int GetContribution(ModifierContext ctx);
}