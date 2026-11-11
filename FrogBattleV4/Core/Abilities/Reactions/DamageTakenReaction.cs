using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Reactions;

public class DamageTakenReaction : TargetedReaction<DamageTakenEventArgs>
{
    public required DamageMetadata Predicate { get; init; }
    protected override bool MatchesPredicate(EntityUid holder, BattleEnvironment env, DamageTakenEventArgs args)
    {
        return Predicate.Permits(args.Metadata);
    }

    public override void Accept(IReactionVisitor visitor) => visitor.Visit(this);
}