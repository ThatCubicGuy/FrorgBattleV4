using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Reactions;

public class DirectDamageTakenReaction : Reaction<DamageTakenEventArgs>
{
    public required DamageData Predicate { get; init; }
    protected override bool Matches(EntityUid holder, BattleEnvironment env, DamageTakenEventArgs args)
    {
        return args.Target == holder && args.Data.Matches(Predicate);
    }

    public override void Accept(IReactionVisitor visitor) => visitor.Visit(this);
}