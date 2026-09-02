using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Value query classifying a stat.
/// </summary>
public sealed record StatQuery : StaticQuery
{
    /// <summary>ID of the stat to query.</summary>
    public required StatId Stat { get; init; }
}