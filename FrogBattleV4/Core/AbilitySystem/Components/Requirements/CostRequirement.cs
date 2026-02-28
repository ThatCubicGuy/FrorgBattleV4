using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.AbilitySystem.Components.Requirements;

public class CostRequirement : IAbilityRequirementComponent
{
    [NotNull] public required CostComponent Cost { get; init; }

    [Pure]
    public bool IsFulfilled(AbilityExecContext ctx)
    {
        return Cost.GetCostRequests(ctx).All(mc => mc.PreviewMutation(
            new ModifierContext
            {
                Actor = ctx.User,
                Other = ctx.MainTarget,
                Ability = ctx.Definition,
                Rng = ctx.Rng,
            }).Satisfiable());
    }
}