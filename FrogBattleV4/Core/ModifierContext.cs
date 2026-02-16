#nullable enable
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
    BattleMember? Actor = null,
    // Member we take as reference.
    BattleMember? Other = null,
    // Ability being used.
    AbilityDefinition? Ability = null,
    // Targeting type in case we attack the reference.
    TargetingType? Aiming = null,
    // Rng for those who need it.
    System.Random? Rng = null);

public record ModifierQuery<TQuery>(TQuery Query, ModifierDirection Direction) where TQuery : struct;

public static class ModifierResolver
{
    /// <summary>
    /// Adds up all the modifiers that have to do in some way with the given query in the given context.
    /// </summary>
    /// <param name="ctx">Context in which to calculate the pipeline.</param>
    /// <param name="query">Query to resolve.</param>
    /// <typeparam name="TQuery">Type of the query to look up.</typeparam>
    /// <returns>A combined ModifierStack.</returns>
    public static ModifierStack Resolve<TQuery>(this ModifierContext ctx, TQuery query) where TQuery : struct
    {
        var mods = new ModifierStack();

        if (ctx.Actor is { } actor)
        {
            var modQuery = new ModifierQuery<TQuery>(query, ModifierDirection.Self);
            mods = actor.AttachedEffects.Aggregate(mods, (stack, eff) =>
                stack + eff.GetContributingModifiers(modQuery, ctx));

            if (ctx.Ability is { } ability)
            {
                mods = ability.Passives.Aggregate(mods, (stack, provider) =>
                    stack + provider.GetContributingModifiers(modQuery, ctx));
            }
        }

        if (ctx.Other is { } other)
        {
            var modQuery = new ModifierQuery<TQuery>(query, ModifierDirection.Reference);
            var revCtx = new ModifierContext(ctx.Other, ctx.Actor);
            mods = other.AttachedEffects.Aggregate(mods, (stack, eff) =>
                stack + eff.GetContributingModifiers(modQuery, revCtx));

            if (ctx.Aiming is { } aiming)
            {
                var hit = other.Hitbox.Resolve(aiming);
                if (hit.IsHit)
                    mods += hit.Modifier.GetContributingModifiers(modQuery, revCtx);
            }
        }

        return mods;
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
}