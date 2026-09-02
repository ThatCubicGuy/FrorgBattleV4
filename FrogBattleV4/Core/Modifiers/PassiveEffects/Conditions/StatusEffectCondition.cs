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
        try
        {
            return env.GetFighter(Side switch
            {
                AffectedSide.Self => subject,
                AffectedSide.Other => reference ?? throw new InvalidOperationException(),
                _ => throw new NotSupportedException($"Side {Side} not supported")
            }).StatusEffects.Where(Query).Sum(sei => SumStacks ? sei.Stacks : 1);
        }
        catch(InvalidOperationException)
        {
            return 0;
        }
    }
}