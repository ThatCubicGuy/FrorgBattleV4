using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects.Conditions;

public class PoolValueCondition : IConditionComponent
{
    private readonly double _step;

    [NotNull] public required string PoolId { get; init; }
    public required ConditionDirection Direction { get; init; }

    /// <summary>
    /// The starting value that the interval starts being calculated from.
    /// </summary>
    public required double MinValue { get; init; }

    /// <summary>
    /// The final value at which the interval stops being calculated.
    /// </summary>
    public required double MaxValue { get; init; }

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

    [Pure]
    public int GetContribution(EffectInfoContext ctx)
    {
        // Funny ahh type check
        if ((Direction == ConditionDirection.Self ? ctx.Holder as BattleMember : ctx.Other)?.Pools
            .GetValueOrDefault(PoolId) is not { } pool) return 0;
        if (Percent)
        {
            return pool.MaxValue.HasValue
                ? GetPercentageValue(pool, pool.MaxValue.Value)
                : throw new InvalidOperationException(
                    $"Percentage type specified for a pool with no MaxValue! {PoolId}");
        }

        return (int)Math.Floor((Math.Clamp(pool.CurrentValue, MinValue, MaxValue) - MinValue) / Step);
    }

    /// <summary>
    /// Wrapper for percentage pool calculations since MaxValue is nullable,
    /// and it would be a bit of a nightmare to cast it everywhere.
    /// </summary>
    /// <param name="pool">Pool to calculate percentage for.</param>
    /// <param name="poolMaxValue">Non-nullable max value of the pool.</param>
    /// <returns>Contribution value for a percentage cost.</returns>
    private int GetPercentageValue(CharacterSystem.Pools.IPoolComponent pool, double poolMaxValue)
    {
        if (!Percent)
            throw new InvalidOperationException(
                "Using GetValuePercentage() inside non-percent cost! What are you doing ??");
        return (int)Math.Floor(
            (Math.Clamp(pool.CurrentValue / poolMaxValue, MinValue, MaxValue) - MinValue) * poolMaxValue / Step);
    }
}