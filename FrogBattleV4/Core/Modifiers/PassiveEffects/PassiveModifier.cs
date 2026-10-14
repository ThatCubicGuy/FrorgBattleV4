using System;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Modifiers.PassiveEffects.Conditions;

namespace FrogBattleV4.Core.Modifiers.PassiveEffects;

/// <summary>
/// Defines a modifier collection that only applies in certain cases.
/// </summary>
public class PassiveModifier : ApplicableEffect
{
    /// <summary>
    /// List of conditions that enable the PassiveModifier's stacks.
    /// </summary>
    public ImmutableList<IConditionComponent> Conditions { get; init; } = [];

    public AccumulationType ConditionAccumulationType { get; init; } = AccumulationType.Min;

    [Pure]
    protected override int GetStacks(EntityUid subject, EntityUid? reference, BattleEnvironment env)
    {
        return Math.Max(0, ConditionAccumulationType switch
        {
            AccumulationType.Min => Conditions.Select(cc => cc.GetContribution(subject, reference, env)).Min(),
            AccumulationType.Max => Conditions.Select(cc => cc.GetContribution(subject, reference, env)).Max(),
            AccumulationType.Sum => Conditions.Select(cc => cc.GetContribution(subject, reference, env)).Sum(),
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
    Min,
    /// <summary>
    /// Takes the highest value from the list of conditions.
    /// </summary>
    Max,
    /// <summary>
    /// Adds up the values of each separate condition.
    /// </summary>
    Sum
}