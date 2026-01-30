using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.Pipelines;

internal static class PipelineExtensions
{
    /// <summary>
    /// Aggregates the full modifier values for a list of effects in the given context. 
    /// </summary>
    /// <param name="effects">The list of effects to calculate the modifiers for.</param>
    /// <param name="ctx">The context in which to calculate the modifiers.</param>
    /// <param name="effCtx">The effect context in which to calculate the modifiers.</param>
    /// <typeparam name="TContext">The type of the given context.</typeparam>
    /// <returns>The final <see cref="ModifierStack"/> calculated from every effect in the list.</returns>
    [Pure]
    public static ModifierStack AggregateMods<TContext>([NotNull] this IReadOnlyList<IAttributeModifier> effects,
        TContext ctx, EffectContext effCtx) where TContext : struct

    {
        // Init mod "sum"
        var finalMods = new ModifierStack();
        foreach (var effect in effects)
        {
            var effectMods = default(ModifierStack);
            // Get the effect-wide mods
            var baseMods = effect.Modifiers.Aggregate(default(ModifierStack),
                (current, mod) => current.AddAll(mod.GetContribution(ctx)));
            // Apply stacks
            for (var i = 0; i < effect.GetStacks(effCtx); i++)
            {
                effectMods = effectMods.AddAll(baseMods);
            }
            // Add the mods from this effect to the sum
            finalMods = finalMods.AddAll(effectMods);
        }
        // Final result is returned
        return finalMods;
    }
}