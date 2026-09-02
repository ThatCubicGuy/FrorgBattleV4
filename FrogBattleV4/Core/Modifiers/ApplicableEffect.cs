using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

/// <summary>
/// Encapsulates an effect that may be applied to a battle member.
/// This effect may have context-dependent stacks. 
/// </summary>
public abstract class ApplicableEffect : IModifierProvider
{
    protected ModifierRuleCollection ModifierRules { get; }

    [Pure]
    protected abstract int GetStacks(EntityUid subject, EntityUid? reference, BattleEnvironment env);

    [Pure]
    public ModifierStack GetContributingModifiers(IQuery query, BattleEnvironment env, ModifierContext ctx)
    {
        return ModifierRules.GetContributingModifiers(query, env, ctx) * GetStacks(query.Main, query.Other, env);
    }
}