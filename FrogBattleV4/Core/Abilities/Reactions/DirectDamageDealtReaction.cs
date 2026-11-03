using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Reactions;

public class DirectDamageDealtReaction : Reaction<DamageTakenEventArgs>
{
    public required DamageData Predicate { get; init; }
    protected override bool Matches(EntityUid holder, BattleEnvironment env, DamageTakenEventArgs args)
    {
        return args.Sender == holder && args.Data.Matches(Predicate);
    }

    public override void Accept(IReactionVisitor visitor) => visitor.Visit(this);
}