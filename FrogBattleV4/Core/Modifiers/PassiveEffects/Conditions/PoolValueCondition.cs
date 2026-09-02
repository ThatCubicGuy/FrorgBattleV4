using System;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers.PassiveEffects.Conditions;

public class PoolValueCondition : IConditionComponent
{
    private readonly double _step;

    #region Metadata

    public required PoolId Pool { get; init; }
    public required AffectedSide Direction { get; init; }

    /// <summary>
    /// The starting value that the interval starts being calculated from.
    /// Default is 0.
    /// </summary>
    public required double MinValue { get; init; } = 0;

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
            if (!double.IsRealNumber(value)) throw new ArgumentException("Step value must be real");
            if (double.IsInfinity(value)) throw new ArgumentException("Step cannot be infinity");
            _step = value;
        }
    }

    /// <summary>
    /// Specifies whether the MinValue, MaxValue and Step properties should be treated
    /// as a percentage of the pool's MaxValue instead of flat values.
    /// </summary>
    public bool Percent { get; init; } = false;

    #endregion

    [Pure]
    public int GetContribution(EntityUid subject, EntityUid? reference, BattleEnvironment env)
    {
        var value = env.GetPoolValue(subject, Pool);

        if (Percent)
        {
            value /= new PoolStatQuery
            {
                PoolId = Pool,
                Channel = PoolValueChannel.Max,
                Subject = subject
            }.Calculate(env);
        }

        return (int)Math.Floor(
            (Math.Clamp(value, MinValue, MaxValue) - MinValue) / Step);
    }
}