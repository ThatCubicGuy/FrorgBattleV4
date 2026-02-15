using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.EffectSystem;

namespace FrogBattleV4.Core.Pipelines;

internal static class PipelineExtensions
{
    /// <summary>
    /// Aggregates the full modifier values for a member's effects in the given context. 
    /// </summary>
    /// <param name="query">Query for specific modifiers.</param>
    /// <param name="ctx">The context in which to calculate the modifiers.</param>
    /// <typeparam name="TQuery">Type of the query, matching context.</typeparam>
    /// <returns>The final <see cref="ModifierStack"/> calculated from every effect in the list.</returns>
    [Pure]
    public static ModifierStack AggregateEffectMods<TQuery>(this TQuery query, EffectInfoContext ctx) where TQuery : struct
    {   
        return ctx.Actor?.AttachedEffects.Aggregate(new ModifierStack(),
            (stack, effect) => stack + effect.GetContributingModifiers(query)) ?? new ModifierStack();
    }
}