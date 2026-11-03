using System;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Abilities.Reactions;

public class EffectAppliedReaction : TargetedReaction<EffectAppliedEventArgs>
{
    public StatusEffectPredicate? Predicate { get; init; }
    protected override bool MatchesPredicate(EntityUid holder, BattleEnvironment env, EffectAppliedEventArgs args)
    {
        return true;
    }

    public override void Accept(IReactionVisitor visitor) => visitor.Visit(this);
}

public class StatusEffectPredicate : GamePredicate<StatusEffect>
{
    public override bool Evaluate(StatusEffect value)
    {
        throw new NotImplementedException();
    }
}