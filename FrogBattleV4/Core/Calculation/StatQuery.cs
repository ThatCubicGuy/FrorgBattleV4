using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Value query classifying a stat.
/// </summary>
[method: SetsRequiredMembers]
public record StatQuery : QueryBase
{
    /// <summary>ID of the stat to query.</summary>
    public required StatId Stat { get; init; }
}