using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.Contexts;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.Pipelines;

internal static class PipelineExtensions
{
    /// <summary>
    /// Aggregates the full modifier values for a member's effects in the given context. 
    /// </summary>
    /// <param name="owner">The member whose effects to compute.</param>
    /// <param name="ctx">The context in which to calculate the modifiers.</param>
    /// <param name="target">The target against which to calculate the modifiers.</param>
    /// <typeparam name="TContext">The type of the given context.</typeparam>
    /// <returns>The final <see cref="ModifierStack"/> calculated from every effect in the list.</returns>
    [Pure]
    public static ModifierStack AggregateMods<TContext>([NotNull] this ISupportsEffects owner,
        TContext ctx, BattleMember target) where TContext : struct, IRelationshipContext
    {
        // Initialize mod "sum"
        var finalMods = new ModifierStack();
        foreach (var effect in owner.GetAttachedEffects(ctx))
        {
            var effectMods = new ModifierStack();
            // Skip effects with no modifiers
            if (effect.Modifiers is null) continue;
            // Get the effect-wide mods
            var baseMods = effect.Modifiers.OfType<IModifierComponent<TContext>>().Aggregate(new ModifierStack(),
                (current, mod) => current.AddAll(mod.GetContribution(ctx)));
            // Apply stacks
            for (var i = 0; i < effect.Stacks; i++)
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