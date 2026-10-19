namespace FrogBattleV4.Core.Combat.Commands;

public class QueueTurnCommand : BattleCommand<QueueTurnData>
{
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
}