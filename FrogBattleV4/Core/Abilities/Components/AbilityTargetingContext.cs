using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Abilities.Components;

public record AbilityTargetingContext
{
    /// <summary>
    /// The target of this hit.
    /// </summary>
    public required EntityUid Target { get; init; }

    /// <summary>
    /// The rank of the target signifies whether it
    /// is the primary target (rank 0), secondary, or further.
    /// Especially useful for blast attacks. Most attacks don't go past rank 1.
    /// </summary>
    public required int Rank { get; init; }

    [Pure]
    public void Deconstruct(out EntityUid target, out int rank)
    {
        target = Target;
        rank = Rank;
    }
}

public class TargetingSelection(IEnumerable<AbilityTargetingContext> selections)
    : IEnumerable<AbilityTargetingContext>
{
    private ImmutableList<AbilityTargetingContext> Targets { get; } = selections.ToImmutableList();

    public IEnumerator<AbilityTargetingContext> GetEnumerator() => Targets.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Targets).GetEnumerator();
}