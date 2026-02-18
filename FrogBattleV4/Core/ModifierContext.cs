#nullable enable
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core;

/// <summary>
/// Context that funnels everything about modifiers into one place.
/// </summary>
public record ModifierContext(
    // The actor in this context.
    IBattleMember? Actor = null,
    // Member we take as reference.
    IBattleMember? Other = null,
    // Ability being used.
    AbilityDefinition? Ability = null,
    // Targeting type in case we attack the reference.
    TargetingType? Aiming = null,
    // Rng for those who need it.
    System.Random? Rng = null);

public record ModifierQuery<TQuery>(TQuery Query, CalcDirection Direction) where TQuery : struct;

public record MutModifierQuery<TQuery>(TQuery Query, CalcDirection Direction, MutModifierDirection MutModifierDirection)
    : ModifierQuery<TQuery>(Query, Direction) where TQuery : struct;

public static class ModifierResolver
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
        return ctx.ResolveMut(query, MutModifierDirection.Incoming)
            .ApplyTo(ctx
                .ResolveMut(query, MutModifierDirection.Outgoing)
                .ApplyTo(baseValue));
    }

    /// <summary>
    /// Builder-style method to process multiple queries with
    /// the creation of just one context in expression bodies.
    /// </summary>
    /// <param name="ctx">Context in which to calculate the pipeline.</param>
    /// <param name="query">Query to resolve.</param>
    /// <param name="stack">The output of the pipeline computation.</param>
    /// <typeparam name="TQuery">Type of the query.</typeparam>
    /// <returns>A reference to this modifier context.</returns>
    [Pure]
    public static ModifierContext ResolveMore<TQuery>(this ModifierContext ctx, TQuery query, out ModifierStack stack)
        where TQuery : struct
    {
        stack = ctx.Resolve(query);
        return ctx;
    }

    /// <summary>
    /// Aggregates all modifiers in a collection based on a query.
    /// </summary>
    /// <param name="ctx">Context in which to aggregate.</param>
    /// <param name="modProviders"></param>
    /// <param name="query"></param>
    /// <param name="type"></param>
    /// <typeparam name="TQuery"></typeparam>
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
        ModifierStack Unc(ModifierStack stack)
        {
            return type switch
            {
                AggregationType.Positive => stack.AsPositive(),
                AggregationType.Negative => stack.AsNegative(),
                _ => stack,
            };
        }
    }

    [Pure]
    public static ModifierStack Resolve<TQuery>(this ModifierContext ctx, TQuery query) where TQuery : struct
    {
        var mods = new ModifierStack();

        if (ctx.Actor is { } actor)
        {
            var modQuery = new ModifierQuery<TQuery>(query, CalcDirection.Self);
            mods += ctx.AggregateMods(actor.Effects, modQuery);

            if (ctx.Ability is { } ability)
            {
                mods += ctx.AggregateMods(ability.Passives, modQuery);
            }
        }

        if (ctx.Other is { } other)
        {
            var modQuery = new ModifierQuery<TQuery>(query, CalcDirection.Other);
            var revCtx = new ModifierContext(ctx.Other, ctx.Actor);
            mods += revCtx.AggregateMods(other.Effects, modQuery);

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
    /// Adds up all the modifiers that have to do in some way with the given query in the given context.
    /// </summary>
    /// <param name="ctx">Context in which to calculate the pipeline.</param>
    /// <param name="query">Query to resolve.</param>
    /// <param name="mutDirection">Direction of the mutation, if the query is a mutation query.</param>
    /// <typeparam name="TQuery">Type of the query to look up.</typeparam>
    /// <returns>A combined ModifierStack.</returns>
    [Pure]
    private static ModifierStack ResolveMut<TQuery>(this ModifierContext ctx, TQuery query,
        MutModifierDirection mutDirection) where TQuery : struct
    {
        var mods = new ModifierStack();

        if (ctx.Actor is { } actor)
        {
            var modQuery = new MutModifierQuery<TQuery>(query, CalcDirection.Self, mutDirection);
            mods += ctx.AggregateMods(actor.Effects, modQuery);

            if (ctx.Ability is { } ability)
            {
                mods += ctx.AggregateMods(ability.Passives, modQuery);
            }
        }

        if (ctx.Other is { } other)
        {
            var modQuery = new MutModifierQuery<TQuery>(query, CalcDirection.Other, mutDirection);
            var revCtx = new ModifierContext(ctx.Other, ctx.Actor);
            mods += revCtx.AggregateMods(other.Effects, modQuery);

            if (ctx.Aiming is { } aiming)
            {
                var hit = other.Hitbox.Resolve(aiming);
                if (hit.WouldHit)
                    mods += hit.Modifier.GetContributingModifiers(modQuery, revCtx);
            }
        }

        return mods;
    }

    private enum AggregationType
    {
        Neutral,
        Positive,
        Negative
    }
}