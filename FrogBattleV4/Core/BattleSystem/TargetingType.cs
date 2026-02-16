namespace FrogBattleV4.Core.BattleSystem;

/// <summary>
/// Custom enum with a payload.
/// </summary>
public abstract record TargetingType
{
    // Block inheritance from outside
    private TargetingType() { }

    public sealed record Region(HitboxRegion Value) : TargetingType;

    public sealed record Height(int Value) : TargetingType;

    public static readonly TargetingType Body = new Region(HitboxRegion.Body);
    public static readonly TargetingType WeakPoint = new Region(HitboxRegion.WeakPoint);
    public static readonly TargetingType Ground = new Height(0);
}