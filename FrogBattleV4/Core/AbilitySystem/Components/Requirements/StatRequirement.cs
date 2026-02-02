using System;
using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.AbilitySystem.Components.Requirements;

public record StatRequirement(
    [NotNull] string Stat,
    double MinValue = double.MinValue,
    double MaxValue = double.MaxValue) : IRequirementComponent
{
    public bool IsFulfilled(AbilityExecContext ctx)
    {
        return Math.Clamp(ctx.User.GetStat(Stat), MinValue, MaxValue).Equals(ctx.User.GetStat(Stat));
    }
}