using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.AbilitySystem.Components.Requirements;

public record CostRequirement(
    [NotNull] ICostComponent Cost) : IRequirementComponent
{
    [Pure]
    public bool IsFulfilled(AbilityExecContext ctx)
    {
        return Cost.GetMutationRequests(ctx).All(r => r.PreviewMutation(
            new MutationExecContext
            {
                Holder = ctx.User,
                Other = ctx.MainTarget.Parent as Character
            }).Allowed);
    }
}