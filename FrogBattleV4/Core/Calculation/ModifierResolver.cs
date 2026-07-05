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
    /// <returns>Final value.</returns>
    [Pure]
    public static double Compute(this QueryBase query, double baseValue)
    {
        return query.Resolve().ApplyTo(baseValue);
    }

    /// <summary>
    /// Computes the pipeline for a mutation value [e.g. damage dealt].
    /// </summary>
    /// <param name="query">Mutation query.</param>
    /// <param name="baseValue">Base value of the mutation.</param>
    /// <returns>Final mutation value.</returns>
    [Pure]
    public static double ComputeMut(this QueryBase query, double baseValue)
    {
        // Resolve incoming resistances and penalties for target->attacker
        return query.ResolveMut(MutModifierDirection.Incoming)
            .ApplyTo(query
                // Resolve outgoing bonuses and penalties for attacker->target
                .ResolveMut(MutModifierDirection.Outgoing)
                .ApplyTo(baseValue));
    }
    /// <summary>
    /// Adds up all the modifiers that have to do in some way with the given value query in the given context.
    /// </summary>
    /// <param name="query">Value Query to resolve.</param>
    /// <returns>A combined ModifierStack.</returns>
    [Pure]
    private static ModifierStack Resolve(this QueryBase query)
    {
        var mods = new ModifierStack();

        if (query.Ctx.Actor is { } actor)
        {
            var modQuery = new ModifierQuery(query, CalcDirection.Self);
            mods += ;
            if (query.Ctx.Ability is { } ability)
            {
                mods += ;
            }
        }

        if (query.Ctx.Other is { } other)
        {
            var modQuery = new ModifierQuery(query, CalcDirection.Other);
            var revCtx = new ModifierContext(query.Ctx.Other, query.Ctx.Actor);
            mods += revCtx.AggregateMods(other.Effects.All, modQuery);

            if (query.Ctx.Aiming is { } aiming)
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
    /// <param name="mutQuery">MutQuery to resolve.</param>
    /// <param name="mutDirection">Direction of the mutation query.</param>
    /// <returns>A combined ModifierStack.</returns>
    [Pure]
    private static ModifierStack ResolveMut(this QueryBase mutQuery, MutModifierDirection mutDirection)
    {
        var mods = new ModifierStack();

        if (mutQuery.Ctx.Actor is { } actor)
        {
            var modQuery = new MutModifierQuery(mutQuery, CalcDirection.Self, mutDirection);
            mods += mutQuery.Ctx.AggregateMods(actor.Effects.All, modQuery);

            if (mutQuery.Ctx.Ability is { } ability)
            {
                mods += mutQuery.Ctx.AggregateMods(ability.Components.OfType<IModifierProvider>(), modQuery);
            }
        }

        if (mutQuery.Ctx.Other is { } other)
        {
            var modQuery = new MutModifierQuery(mutQuery, CalcDirection.Other, mutDirection);
            var revCtx = new ModifierContext(mutQuery.Ctx.Other, mutQuery.Ctx.Actor);
            mods += revCtx.AggregateMods(other.Effects.All, modQuery);

            if (mutQuery.Ctx.Aiming is { } aiming)
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
    private static ModifierStack AggregateMods(this ModifierContext ctx,
        IEnumerable<IModifierProvider> modProviders,
        ModifierQuery query,
        AggregationType type = AggregationType.Neutral)
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