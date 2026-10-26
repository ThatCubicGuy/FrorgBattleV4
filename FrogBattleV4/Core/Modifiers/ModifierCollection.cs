using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

/// <summary>
/// Defines methods for aggregating modifier rules and providers into a single modifier stack.
/// </summary>
public static class ModifierCollections
{
    public static ModifierStack GetContributingModifiers(this IEnumerable<IModifierProvider> providers, IQuery query, BattleEnvironment env, ModifierContext ctx)
    {
        return providers.Aggregate(ModifierStack.None,
            (stack, mod) => stack + mod.GetContributingModifiers(query, env, ctx));
    }

    public static ModifierStack GetContributingModifiers(this IEnumerable<IModifierRule> rules, IQuery query, BattleEnvironment env, ModifierContext ctx)
    {
        return rules.Where(rule => rule.AppliesTo(query, ctx)).Select(rule => rule.ModifierStack)
            .Aggregate((current, next) => current + next);
    }
}