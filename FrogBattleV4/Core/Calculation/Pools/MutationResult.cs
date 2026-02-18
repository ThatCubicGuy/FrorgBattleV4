using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.Calculation.Pools;

/// <summary>
/// A fully calculated instance of a specific pool mutation. The raw value of <paramref name="FinalDeltaValue"/>
/// is added to <paramref name="ResultTarget"/>.
/// </summary>
/// <param name="ResultTarget">The pool component that is going to be mutated.</param>
/// <param name="FinalDeltaValue">Final value that will be added to the given pool.</param>
public record MutationResult([NotNull] PoolComponent ResultTarget, double FinalDeltaValue) : IResultContext<PoolComponent>;

internal static class MutationResultExtensions
{
    /// <summary>
    /// Determines whether the given pool holds enough resources to sustain this mutation without reaching its limits.
    /// </summary>
    /// <returns>True if the mutation will not reach any limits, false otherwise.</returns>
    public static bool Satisfiable([NotNull] this MutationResult mr) =>
        !(mr.ResultTarget.MaxValue < mr.ResultTarget.CurrentValue + mr.FinalDeltaValue ||
          mr.ResultTarget.MinValue > mr.ResultTarget.CurrentValue + mr.FinalDeltaValue);
    // Use negated greater than / less than because MaxValue and MinValue are nullable.
}