using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects.Conditions;

public class PoolValueCondition : IConditionComponent
{
    private readonly double _step;

    #region Metadata

    [NotNull] public required PoolId PoolId { get; init; }
    public required CalcDirection Direction { get; init; }

    /// <summary>
    /// The starting value that the interval starts being calculated from.
    /// Default is 0.
    /// </summary>
    public required double MinValue { get; init; } = 0;

    /// <summary>
    /// The final value at which the interval stops being calculated.
    /// </summary>
    public required double? MaxValue { get; init; }

    /// <summary>
    /// The step by which the contribution is calculated.
    /// Every Step amount that the value has over MinValue it increases by one, up to (MaxValue - MinValue) / Step.
    /// Cannot be zero.
    /// </summary>
    /// <exception cref="ArgumentException">Step is zero.</exception>
    public required double Step
    {
        get => _step;
        init
        {
            if (value == 0) throw new ArgumentException("Step cannot be zero");
            _step = value;
        }
    }

    /// <summary>
    /// Specifies whether the MinValue and MaxValue properties should be treated
    /// as a percentage of the pool's MaxValue instead of flat values.
    /// </summary>
    public bool Percent { get; init; } = false;

    #endregion

    [Pure]
    public int GetContribution(ModifierContext ctx)
    {
        // Funny ahh type check
        if ((Direction == CalcDirection.Self ? ctx.Actor : ctx.Other)?.Pools[PoolId] is not { } pool) return 0;
        if (Percent)
        {
            return pool.MaxValue.HasValue
                ? (int)Math.Floor((Math.Clamp(pool.CurrentValue / pool.MaxValue.Value,
                    MinValue, MaxValue ?? double.MaxValue) - MinValue) * pool.MaxValue.Value / Step)
                : throw new InvalidOperationException(
                    $"Percentage type specified for a pool with no MaxValue! ({PoolId})");
        }

        return (int)Math.Floor(
            (Math.Clamp(pool.CurrentValue, MinValue, MaxValue ?? double.MaxValue) - MinValue) / Step);
    }
}