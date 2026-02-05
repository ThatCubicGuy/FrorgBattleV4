#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects;

/// <summary>
/// Defines a passive effect that is to be processed 
/// </summary>
public class PassiveEffectDefinition : IEffectDefinition, IModifierContributor
{
    public required string Id { get; init; }
    public required IEnumerable<IModifierRule> Modifiers { get; init; }

    /// <summary>
    /// List of conditions that enable the PassiveEffect's stacks.
    /// </summary>
    public required IEnumerable<IConditionComponent> Conditions { get; init; }

    public AccumulationType ConditionAccumulationType { get; set; }

    [Pure]
    private uint GetStacks(EffectInfoContext ctx)
    {
        return (uint)Math.Max(0, ConditionAccumulationType switch
        {
            AccumulationType.And => Conditions.Select(cc => cc.GetContribution(ctx)).Min(),
            AccumulationType.Or => Conditions.Select(cc => cc.GetContribution(ctx)).Max(),
            AccumulationType.Accumulate => Conditions.Select(cc => cc.GetContribution(ctx)).Sum(),
            _ => throw new InvalidEnumArgumentException(nameof(ConditionAccumulationType),
                (int)ConditionAccumulationType, typeof(AccumulationType))
        });
    }

    [Pure]
    public EffectInstanceContext GetInstance(EffectInfoContext ctx)
    {
        return new EffectInstanceContext
        {
            EffectId = Id,
            Holder = ctx.Holder,
            Target = ctx.Other,
            EffectSource = null,
            Stacks = GetStacks(ctx),
            Modifiers = Modifiers,
            Mutators = null,
        };
    }

    [Pure]
    public ModifierStack GetContributions<TQuery>(EffectInfoContext ctx, TQuery query)
        where TQuery : struct
    {
        return Modifiers.Where(mr => mr.AppliesFor(query))
            .Aggregate(new ModifierStack(), (stack, rule) =>
                stack.Add(rule.ModifierStack.MultiplyBy(GetStacks(ctx))));
    }
}

public enum AccumulationType
{
    /// <summary>
    /// Takes the lowest value from the list of conditions.
    /// </summary>
    And,
    /// <summary>
    /// Takes the highest value from the list of conditions.
    /// </summary>
    Or,
    /// <summary>
    /// Adds up the values of each separate condition.
    /// </summary>
    Accumulate
}