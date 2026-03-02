using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Effects.Modifiers;
using FrogBattleV4.Core.Effects.StatusEffects;

namespace FrogBattleV4.Core.Effects.PassiveEffects.Conditions;

public class StatusEffectCondition : IConditionComponent
{
    [NotNull] public required System.Func<StatusEffectInstance, bool> Query { get; init; }
    public required CalcDirection Direction { get; init; }
    public bool SumStacks { get; init; }

    [Pure]
    public int GetContribution(ModifierContext ctx)
    {
        return (Direction switch
        {
            CalcDirection.Self => ctx.Actor,
            CalcDirection.Other => ctx.Other,
            _ => null
        })?.Effects.StatusEffects.Where(Query).Sum(sei => SumStacks? sei.Stacks : 1) ?? 0;
    }
}