namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Value query classifying a stat.
/// </summary>
public sealed class StatQuery : AttributeQuery
{
    /// <summary>ID of the stat to query.</summary>
    public required StatName Stat { get; init; }
}