using System;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.AbilitySystem.Components.Requirements;

public record StatRequirement(
    StatId Stat,
    double? MinValue = null,
    double? MaxValue = null) : IRequirementComponent
{
    public bool IsFulfilled(AbilityExecContext ctx)
    {
        return new ModifierContext
        {
            Actor = ctx.User,
            Other = ctx.MainTarget,
            Ability = ctx.Definition,
            Rng = ctx.Rng,
        }.ComputeStat(Stat).IsWithinRange(MinValue, MaxValue);
    }
}