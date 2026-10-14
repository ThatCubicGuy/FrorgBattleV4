using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Modifiers;

/// <summary>
/// Encapsulates an effect that may be applied to a battle member.
/// This effect may have context-dependent stacks. 
/// </summary>
public abstract class ApplicableEffect : IModifierProvider
{
    public IEnumerable<ModifierRule> ModifierRules { get; init; } = [];
    public IEnumerable<IMutatorComponent> Mutators { get; init; } = [];

    [Pure]
    protected abstract int GetStacks(EntityUid subject, EntityUid? reference, BattleEnvironment env);

    [Pure]
    public ModifierStack GetContributingModifiers(IQuery query, BattleEnvironment env, ModifierContext ctx)
    {
        return ModifierRules.GetContributingModifiers(query, env, ctx) * GetStacks(query.Main, query.Other, env);
    }
}

public static class ModifierRuleExtensions
{
    /// <summary>
    /// Aggregates all contained modifiers that contribute to this query.
    /// </summary>
    /// <param name="rules">Enumerable of modifier rules.</param>
    /// <param name="query">Query to search for.</param>
    /// <param name="env">Environment to aggregate in.</param>
    /// <param name="ctx">Modifier context to search in regard to.</param>
    /// <returns>The aggregated ModifierStack.</returns>
    [Pure]
    public static ModifierStack GetContributingModifiers(this IEnumerable<ModifierRule> rules, IQuery query, BattleEnvironment env, ModifierContext ctx)
    {
        return rules.Where(mr => mr.Applies(query, ctx))
            .Aggregate(new ModifierStack(), (stack, rule) =>
                stack + rule.ModifierStack);
    }
}