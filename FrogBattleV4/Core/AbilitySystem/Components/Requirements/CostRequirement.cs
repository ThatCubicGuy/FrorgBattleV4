using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.AbilitySystem.Components.Requirements;

public record CostRequirement([NotNull] ICostComponent Cost) : IRequirementComponent
{
    [Pure]
    public bool IsFulfilled(AbilityExecContext ctx)
    {
        return Cost.GetCostRequests(ctx).All(mr => mr.PreviewMutation(
            new ModifierContext(ctx.User, ctx.MainTarget)).Allowed);
    }
}