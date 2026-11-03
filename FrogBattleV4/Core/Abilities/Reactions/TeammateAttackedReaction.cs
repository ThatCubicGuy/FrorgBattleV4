using System.Linq;

namespace FrogBattleV4.Core.Abilities.Reactions;

public class TeammateAttackedReaction : Reaction<DamageTakenEventArgs>
{
    protected override bool Matches(EntityUid holder, BattleEnvironment env, DamageTakenEventArgs args)
    {
        return env.GetNeighbors(args.Target).Contains(holder);
    }

    public override void Accept(IReactionVisitor visitor) => visitor.Visit(this);
}