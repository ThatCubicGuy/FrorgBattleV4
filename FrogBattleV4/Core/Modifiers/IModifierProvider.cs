using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

/// <summary>
/// Represents a full AttributeModifier wrapper that returns a total ModifierStack for a query in a context.
/// </summary>
public interface IModifierProvider
{
    /// <summary>
    /// Gets the contribution of this IModifierComponent to the given modifier query.
    /// This method applies stacks if necessary.
    /// </summary>
    /// <param name="query">Query for which to check contributions.</param>
    /// <param name="env">Environment in which to process the query.</param>
    /// <param name="ctx">ModifierContext of the current calculation.</param>
    /// <returns>A modifier stack revealing the final contribution.</returns>
    [Pure]
    ModifierStack GetContributingModifiers(IQuery query, BattleEnvironment env, ModifierContext ctx);
}