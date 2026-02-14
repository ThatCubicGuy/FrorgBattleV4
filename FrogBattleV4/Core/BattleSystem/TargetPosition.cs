namespace FrogBattleV4.Core.BattleSystem;

/// <summary>
/// Stores the position of a targetable part for any battle member.
/// Currently, this can be elevation and depth.
/// </summary>
/// <param name="Height">How high the part sits, starting from the ground.</param>
public readonly record struct TargetPosition(int Height = 0)
{
    public static implicit operator TargetPosition(int height) => new(height);
}