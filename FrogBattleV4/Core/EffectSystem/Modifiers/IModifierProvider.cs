using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.EffectSystem.Modifiers;

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
    /// <param name="ctx">Context (holder/target) in which to check contributions.</param>
    /// <typeparam name="TQuery">Type of query to handle.</typeparam>
    /// <returns>A modifier stack revealing the final contribution.</returns>
    [Pure]
    ModifierStack GetContributingModifiers<TQuery>(ModifierQuery<TQuery> query, ModifierContext ctx)
        where TQuery : struct;
}