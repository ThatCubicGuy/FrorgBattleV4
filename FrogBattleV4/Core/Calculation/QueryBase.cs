namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Base class for all queries containing some methods.
/// </summary>
public abstract record QueryBase
{
    /// <summary>
    /// Context in which to query.
    /// </summary>
    public required ModifierContext Ctx { get; init; }
}