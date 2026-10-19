using System;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Modifiers.PassiveEffects.Conditions;

namespace FrogBattleV4.Core.Modifiers.PassiveEffects;

/// <summary>
/// Defines a modifier collection that only applies in certain cases.
/// </summary>
public class PassiveModifier : IModifierProvider
{
    public required ModifierCollection Modifiers { get; init; }
    /// <summary>
    /// List of conditions that enable the PassiveModifier's stacks.
    /// </summary>
    public required ConditionCollection Conditions { get; init; }

    [Pure]
    protected int GetStacks(EntityUid subject, EntityUid? reference, BattleEnvironment env)
    {
        return Math.Max(0, Conditions.GetContribution(subject, reference, env));
    }

    [Pure]
    public ModifierStack GetContributingModifiers(IQuery query, BattleEnvironment env, ModifierContext ctx)
    {
        return Modifiers.GetContributingModifiers(query, env, ctx) * GetStacks(query.Main, query.Other, env);
    }
}