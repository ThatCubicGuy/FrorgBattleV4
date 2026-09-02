using System;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Modifiers.PassiveEffects.Conditions;

namespace FrogBattleV4.Core.Modifiers.PassiveEffects;

/// <summary>
/// Defines a passive effect that is to be processed 
/// </summary>
public class PassiveEffectDefinition : ApplicableEffect
{
    /// <summary>
    /// List of conditions that enable the PassiveEffect's stacks.
    /// </summary>
    public ImmutableList<IConditionComponent> Conditions { get; } = [];

    public AccumulationType ConditionAccumulationType { get; set; } = AccumulationType.And;

    [Pure]
    protected override int GetStacks(EntityUid subject, EntityUid? reference, BattleEnvironment env)
    {
        return Math.Max(0, ConditionAccumulationType switch
        {
            AccumulationType.And => Conditions.Select(cc => cc.GetContribution(subject, reference, env)).Min(),
            AccumulationType.Or => Conditions.Select(cc => cc.GetContribution(subject, reference, env)).Max(),
            AccumulationType.Accumulate => Conditions.Select(cc => cc.GetContribution(subject, reference, env)).Sum(),
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