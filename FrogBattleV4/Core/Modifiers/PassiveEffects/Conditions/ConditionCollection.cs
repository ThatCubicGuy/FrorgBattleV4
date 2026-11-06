using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Modifiers.PassiveEffects.Conditions;

public record ConditionCollection(ImmutableList<IConditionComponent> Conditions) : IConditionComponent, IReadOnlyCollection<IConditionComponent>
{
    public static readonly ConditionCollection Empty = new(ImmutableList<IConditionComponent>.Empty);

    public AccumulationType Accumulation { get; init; } = AccumulationType.Min;

    public int GetContribution(EntityUid subject, EntityUid? reference, BattleEnvironment env)
    {
        // Empty conditions list makes the passive act as a permanent status effect.
        // Useful for stats.
        if (Conditions.Count == 0) return 1;
        var contributions = Conditions.Select(cc => cc.GetContribution(subject, reference, env));
        return Accumulation switch
        {
            AccumulationType.Min => contributions.Min(),
            AccumulationType.Max => contributions.Max(),
            AccumulationType.Sum => contributions.Sum(),
            _ => throw new InvalidEnumArgumentException(nameof(Accumulation),
                (int)Accumulation, typeof(AccumulationType)),
        };
    }

    public IEnumerator<IConditionComponent> GetEnumerator() => Conditions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Conditions).GetEnumerator();
    public int Count => Conditions.Count;
}

[JsonConverter(typeof(JsonStringEnumConverter<AccumulationType>))]
public enum AccumulationType
{
    /// <summary>
    /// Takes the lowest value from the list of conditions.
    /// </summary>
    [JsonStringEnumMemberName("min")]
    Min,
    /// <summary>
    /// Takes the highest value from the list of conditions.
    /// </summary>
    [JsonStringEnumMemberName("max")]
    Max,
    /// <summary>
    /// Adds up the values of each separate condition.
    /// </summary>
    [JsonStringEnumMemberName("sum")]
    Sum,
}