using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.EffectSystem;

public interface IModifierContributor
{
    /// <summary>
    /// Gets the contribution of this IModifierContributor to the given query, in the given context.
    /// This method applies stacks if necessary.
    /// </summary>
    /// <param name="ctx">Context in which to get contributions</param>
    /// <param name="query">Query to which to contribute.</param>
    /// <typeparam name="TQuery">Type of the query struct.</typeparam>
    /// <returns>A modifier stack revealing the final contribution.</returns>
    [Pure]
    ModifierStack GetContributions<TQuery>(EffectInfoContext ctx, TQuery query)
        where TQuery : struct;
}