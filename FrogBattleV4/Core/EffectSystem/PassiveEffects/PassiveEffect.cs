#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects;

public class PassiveEffect : IEffectDefinition, IAttributeModifier
{
    public required string Id { get; init; }
    public required IReadOnlyList<IModifierComponent> Modifiers { get; init; }
    /// <summary>
    /// List of conditions that enable the PassiveEffect's stacks.
    /// </summary>
    public required IReadOnlyList<IConditionComponent> Conditions { get; init; }

    public AccumulationType ConditionAccumulationType { get; set; }

    public uint GetStacks(EffectContext ctx)
    {
        return (uint)(ConditionAccumulationType switch
        {
            AccumulationType.And => Conditions.Select(x => x.GetContribution(ctx)).Min(),
            AccumulationType.Or => Conditions.Select(x => x.GetContribution(ctx)).Max(),
            AccumulationType.Accumulate => Math.Max(0, Conditions.Select(x => x.GetContribution(ctx)).Sum()),
            _ => throw new InvalidEnumArgumentException(nameof(ConditionAccumulationType), (int)ConditionAccumulationType, typeof(AccumulationType))
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