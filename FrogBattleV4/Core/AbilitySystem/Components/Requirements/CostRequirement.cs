using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.AbilitySystem.Components.Requirements;

public class CostRequirement : CostComponent, IAbilityRequirementComponent
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

    // CostComponent is inherited to avoid writing costs twice - you just create a new
    // CostRequirement with a new cost and there you go. no more double adding things.
    public override IEnumerable<MutationCommand> GetCostRequests(AbilityExecContext ctx) => Cost.GetCostRequests(ctx);
}