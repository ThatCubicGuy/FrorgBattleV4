using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Modifiers.PassiveEffects.Conditions;

public class StatusEffectCondition : IConditionComponent
{
    public required System.Func<StatusEffectInstance, bool> Query { get; init; }
    public required AffectedSide Side { get; init; }
    public bool SumStacks { get; init; }

    [Pure]
    public int GetContribution(RelationContext ctx)
    {
        return (Side switch
        {
            AffectedSide.Self => ctx.Actor,
            AffectedSide.Other => ctx.Target,
            _ => null
        })?.Effects.StatusEffects.Where(Query).Sum(sei => SumStacks? sei.Stacks : 1) ?? 0;
    }
}