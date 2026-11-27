namespace FrogBattleV4.Core.Combat.Commands;

public class EnqueueActionCommand : BattleCommand
{
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    public required TurnDecision Decision { get; init; }
    public required ActionType Type { get; init; }
}