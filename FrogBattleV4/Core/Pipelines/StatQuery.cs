using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.Pipelines;

/// <summary>
/// Value query classifying a stat.
/// </summary>
/// <param name="stat"></param>
[method: SetsRequiredMembers]
public readonly struct StatQuery(StatId stat)
{
    public required StatId Stat { get; init; } = stat;
}