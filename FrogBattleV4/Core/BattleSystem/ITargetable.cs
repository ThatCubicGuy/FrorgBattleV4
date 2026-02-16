#nullable enable
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.BattleSystem;

public interface ITargetable
{
    /// <summary>
    /// Resolve a selection for this type of targeting.
    /// </summary>
    /// <param name="targeting"></param>
    /// <returns>The result of being targeted in this way.</returns>
    [Pure]
    TargetingResult Resolve(TargetingType targeting);
}

public readonly record struct TargetingResult(bool IsHit, IModifierProvider Modifier)
{
    public static readonly TargetingResult Miss = new() { IsHit = false };
}

public enum HitboxRegion
{
    Body,
    WeakPoint
}

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