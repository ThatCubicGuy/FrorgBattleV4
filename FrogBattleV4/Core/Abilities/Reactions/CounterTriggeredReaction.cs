using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Reactions;

// WIP
public class CounterTriggeredReaction : Reaction
{
    public required CounterName Counter { get; init; }

    public override void Accept(IReactionVisitor visitor) => visitor.Visit(this);
}