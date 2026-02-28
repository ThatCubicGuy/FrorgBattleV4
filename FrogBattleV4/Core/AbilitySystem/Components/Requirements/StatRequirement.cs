using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.AbilitySystem.Components.Requirements;

public class StatRequirement : IAbilityRequirementComponent
{
    public required StatId Stat { get; init; }
    public double? MinValue { get; init; } = null;
    public double? MaxValue { get; init; } = null;

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