using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Reactions;

public class PoolMutatedReaction : TargetedReaction<PoolMutatedEventArgs>
{
    public required PoolName Pool { get; init; }
    protected override bool MatchesPredicate(EntityUid holder, BattleEnvironment env, PoolMutatedEventArgs args)
    {
        return args.Pool == Pool;
    }

    public override void Accept(IReactionVisitor visitor) => visitor.Visit(this);
}