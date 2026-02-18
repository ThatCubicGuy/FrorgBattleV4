#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.EffectSystem.PassiveEffects.Conditions;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects;

/// <summary>
/// Defines a passive effect that is to be processed 
/// </summary>
public class PassiveEffectDefinition : ApplicableEffect
{
    public required string Id { get; init; }
    public required ModifierCollection Modifiers { get; init; }

    /// <summary>
    /// List of conditions that enable the PassiveEffect's stacks.
    /// </summary>
    public required IEnumerable<IConditionComponent> Conditions { get; init; }

    public AccumulationType ConditionAccumulationType { get; set; } = AccumulationType.And;

    protected override ModifierCollection ModifierCollection => Modifiers;
    [Pure]
    protected override int GetStacks(ModifierContext ctx)
    {
        return Math.Max(0, ConditionAccumulationType switch
        {
            AccumulationType.And => Conditions.Select(cc => cc.GetContribution(ctx)).Min(),
            AccumulationType.Or => Conditions.Select(cc => cc.GetContribution(ctx)).Max(),
            AccumulationType.Accumulate => Conditions.Select(cc => cc.GetContribution(ctx)).Sum(),
            _ => throw new System.ComponentModel.InvalidEnumArgumentException(nameof(ConditionAccumulationType),
                (int)ConditionAccumulationType, typeof(AccumulationType))
        });
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