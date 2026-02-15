#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.EffectSystem.StatusEffects;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects.Conditions;

public class StatusEffectCondition : IConditionComponent
{
    [NotNull] public required StatusEffectQuery Query { get; init; }
    public required ConditionDirection Direction { get; init; }

    [Pure]
    public int GetContribution(EffectInfoContext ctx)
    {
        return (Direction switch
        {
            ConditionDirection.Self => ctx.Actor,
            ConditionDirection.Other => ctx.Other,
            _ => null
        })?.AttachedEffects.OfType<StatusEffectInstance>().Where(Query.Invoke).Sum(sei => sei.Stacks) ?? 0;
    }
}

public delegate bool StatusEffectQuery(StatusEffectInstance eff);