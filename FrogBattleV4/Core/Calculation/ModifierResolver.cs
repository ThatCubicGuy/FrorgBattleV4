#nullable enable
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Effects.Modifiers;

namespace FrogBattleV4.Core.Calculation;

internal static class ModifierResolver
{
    /// <summary>
    /// Computes the pipeline for a still value [e.g. a stat].
    /// </summary>
    /// <param name="query">Value query.</param>
    /// <param name="baseValue">Base value of the stat.</param>
    /// <param name="ctx">Context in which to compute.</param>
    /// <typeparam name="TQuery">Type of the value query.</typeparam>
    /// <returns>Final value.</returns>
    [Pure]
    public static double Compute<TQuery>(this TQuery query, double baseValue, ModifierContext ctx) where TQuery : struct
    {
        return ctx.Resolve(query).ApplyTo(baseValue);
    }

    /// <summary>
    /// Computes the pipeline for a mutation value [e.g. damage dealt].
    /// </summary>
    /// <param name="query">Mutation query.</param>
    /// <param name="baseValue">Base value of the mutation.</param>
    /// <param name="ctx">ModifierCotext in which to compute the value.</param>
    /// <typeparam name="TQuery"></typeparam>
    /// <returns>Final mutation value.</returns>
    [Pure]
    public static double ComputeMut<TQuery>(this TQuery query, double baseValue, ModifierContext ctx)
        where TQuery : struct
    {
        // Resolve incoming resistances and penalties for target->attacker
        return new ModifierContext(ctx.Other, ctx.Actor)
            .ResolveMut(query, MutModifierDirection.Incoming)
            .ApplyTo(ctx
                // Resolve outgoing bonuses and penalties for attacker->target
                .ResolveMut(query, MutModifierDirection.Outgoing)
                .ApplyTo(baseValue));
    }
    /// <summary>
    /// Adds up all the modifiers that have to do in some way with the given value query in the given context.
    /// </summary>
    /// <param name="ctx">Context in which to calculate the pipeline.</param>
    /// <param name="query">Value Query to resolve.</param>
    /// <typeparam name="TQuery">Type of the query to look up.</typeparam>
    /// <returns>A combined ModifierStack.</returns>
    [Pure]
    private static ModifierStack Resolve<TQuery>(this ModifierContext ctx, TQuery query) where TQuery : struct
    {
        var mods = new ModifierStack();

        if (ctx.Actor is { } actor)
        {
            var modQuery = new ModifierQuery<TQuery>(query, CalcDirection.Self);
            mods += ctx.AggregateMods(actor.Effects.All, modQuery);
            if (ctx.Ability is { } ability)
            {
                mods += ctx.AggregateMods(ability.Components.OfType<IModifierProvider>(), modQuery);
            }
        }

        if (ctx.Other is { } other)
        {
            var modQuery = new ModifierQuery<TQuery>(query, CalcDirection.Other);
            var revCtx = new ModifierContext(ctx.Other, ctx.Actor);
            mods += revCtx.AggregateMods(other.Effects.All, modQuery);

            if (ctx.Aiming is { } aiming)
            {
                var hit = other.Hitbox.Resolve(aiming);
                if (hit.WouldHit)
                    mods += hit.Modifier.GetContributingModifiers(modQuery, revCtx);
            }
        }

        return mods;
    }

    /// <summary>
    /// Adds up all the modifiers that have to do in some way with the given mutation query in the given context.
    /// </summary>
    /// <param name="ctx">Context in which to calculate the pipeline.</param>
    /// <param name="mutQuery">MutQuery to resolve.</param>
    /// <param name="mutDirection">Direction of the mutation query.</param>
    /// <typeparam name="TQuery">Type of the query to look up.</typeparam>
    /// <returns>A combined ModifierStack.</returns>
    [Pure]
    private static ModifierStack ResolveMut<TQuery>(this ModifierContext ctx, TQuery mutQuery,
        MutModifierDirection mutDirection) where TQuery : struct
    {
        var mods = new ModifierStack();

        if (ctx.Actor is { } actor)
        {
            var modQuery = new MutModifierQuery<TQuery>(mutQuery, CalcDirection.Self, mutDirection);
            mods += ctx.AggregateMods(actor.Effects.All, modQuery);

            if (ctx.Ability is { } ability)
            {
                mods += ctx.AggregateMods(ability.Components.OfType<IModifierProvider>(), modQuery);
            }
        }

        if (ctx.Other is { } other)
        {
            var modQuery = new MutModifierQuery<TQuery>(mutQuery, CalcDirection.Other, mutDirection);
            var revCtx = new ModifierContext(ctx.Other, ctx.Actor);
            mods += revCtx.AggregateMods(other.Effects.All, modQuery);

            if (ctx.Aiming is { } aiming)
            {
                var hit = other.Hitbox.Resolve(aiming);
                if (hit.WouldHit)
                    mods += hit.Modifier.GetContributingModifiers(modQuery, revCtx);
            }
        }

        return mods;
    }

    /// <summary>
    /// Aggregates all modifiers in a collection based on a query.
    /// </summary>
    /// <param name="ctx">Context in which to aggregate.</param>
    /// <param name="modProviders">THe list of modifier providers to aggregate.</param>
    /// <param name="query">Query for each modifier.</param>
    /// <param name="type">If specified, applies AsPositive or AsNegative to the stack after each addition.</param>
    /// <typeparam name="TQuery">Type of the processed query.</typeparam>
    /// <returns>An aggregated ModifierStack.</returns>
    [Pure]
    private static ModifierStack AggregateMods<TQuery>(this ModifierContext ctx,
        IEnumerable<IModifierProvider> modProviders,
        ModifierQuery<TQuery> query,
        AggregationType type = AggregationType.Neutral) where TQuery : struct
    {
        return modProviders.Aggregate(new ModifierStack(), (stack, eff) =>
            Unc(stack + eff.GetContributingModifiers(query, ctx)));

        // unc fixing my modifiers :100:
        ModifierStack Unc(ModifierStack stack) => type switch
        {
            AggregationType.Positive => stack.AsPositive(),
            AggregationType.Negative => stack.AsNegative(),
            _ => stack,
        };
    }

    private enum AggregationType
    {
        Neutral,
        Positive,
        Negative
    }
}