using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Requirements;

public record CostRequirement([NotNull] ICostComponent Cost) : IRequirementComponent
{
    [Pure]
    public bool IsFulfilled(AbilityExecContext ctx)
    {
        return Cost.GetCostRequests(ctx).All(mr => mr.PreviewMutation(
            new ModifierContext
            {
                Actor = ctx.User,
                Other = ctx.MainTarget,
                Ability = ctx.Definition,
                Rng = ctx.Rng,
            }).Satisfiable());
    }
}