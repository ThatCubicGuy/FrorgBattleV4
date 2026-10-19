using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;

namespace FrogBattleV4.Core.Modifiers.PassiveEffects.Conditions;

public record ConditionCollection(ImmutableList<IConditionComponent> Conditions) : IConditionComponent
{
    public AccumulationType ConditionAccumulationType { get; init; } = AccumulationType.Min;

    public int GetContribution(EntityUid subject, EntityUid? reference, BattleEnvironment env)
    {
        var contributions = Conditions.Select(cc => cc.GetContribution(subject, reference, env));
        return ConditionAccumulationType switch
        {
            AccumulationType.Min => contributions.Min(),
            AccumulationType.Max => contributions.Max(),
            AccumulationType.Sum => contributions.Sum(),
            _ => throw new InvalidEnumArgumentException(nameof(ConditionAccumulationType),
                (int)ConditionAccumulationType, typeof(AccumulationType))
        };
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