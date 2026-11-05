using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Abilities.Reactions;

public class EffectAppliedReaction : TargetedReaction<EffectAppliedEventArgs>
{
    public StatusEffectPredicate? Predicate { get; init; }
    protected override bool MatchesPredicate(EntityUid holder, BattleEnvironment env, EffectAppliedEventArgs args)
    {
        return Predicate.SafeEvaluate(args);
    }

    public override void Accept(IReactionVisitor visitor) => visitor.Visit(this);
}