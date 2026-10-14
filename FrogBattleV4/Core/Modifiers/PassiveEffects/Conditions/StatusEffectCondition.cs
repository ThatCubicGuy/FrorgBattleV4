using System;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Modifiers.PassiveEffects.Conditions;

public class StatusEffectCondition : IConditionComponent
{
    public required Func<StatusEffectInstance, bool> Query { get; init; }
    public required AffectedSide Side { get; init; }
    public bool SumStacks { get; init; }

    [Pure]
    public int GetContribution(EntityUid subject, EntityUid? reference, BattleEnvironment env)
    {
        return env.GetStatusEffects(subject).Where(Query).Count();
    }
}