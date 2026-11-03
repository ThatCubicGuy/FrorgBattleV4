namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Represents event data for an event that has a meaningful "delta" value (e.g. damage, mutation).
/// </summary>
public interface IDeltaEventArgs : IBattleCommandEventArgs
{
    double Delta { get; }
}