using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.CharacterSystem.Components;

namespace FrogBattleV4.Core.AbilitySystem.Components.Requirements;

public class PoolRequirementComponent : IRequirementComponent
{
    [NotNull] public required string PoolId { get; init; }
    public required double Amount { get; init; }

    public bool Check(AbilityExecContext ctx)
    {
        return ctx.User.Pools[PoolId].ProcessSpend(Amount, new SpendContext
        {
            Ability = ctx.Definition,
            Owner = ctx.User,
            Mode = SpendMode.Validate
        }).Sufficient;
    }
}