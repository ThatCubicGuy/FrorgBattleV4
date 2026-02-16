using System;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.AbilitySystem.Components.Requirements;

public record StatRequirement(
    StatId Stat,
    double MinValue = double.MinValue,
    double MaxValue = double.MaxValue) : IRequirementComponent
{
    public bool IsFulfilled(AbilityExecContext ctx)
    {
        return Math.Clamp(new ModifierContext(ctx.User).ComputeStat(Stat), MinValue, MaxValue)
            .Equals(new ModifierContext(ctx.User).ComputeStat(Stat));
    }
}