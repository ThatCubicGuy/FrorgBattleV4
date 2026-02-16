using System;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects.Conditions;

public class StatValueCondition : IConditionComponent
{
    private readonly double _step;

    /// <summary>
    /// Stat to query for.
    /// </summary>
    public required StatId Stat { get; set; }

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

    [Pure]
    public int GetContribution(ModifierContext ctx)
    {
        if (ctx.Actor is not { } actor) return 0;
        return (int)Math.Floor((Math.Clamp(ctx.ComputeStat(Stat), MinValue, MaxValue) - MinValue) / Step);
    }
}