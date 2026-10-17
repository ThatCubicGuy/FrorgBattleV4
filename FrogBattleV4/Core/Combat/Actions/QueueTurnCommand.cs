namespace FrogBattleV4.Core.Combat.Actions;

public class QueueTurnCommand : BattleCommand<QueueTurnData>
{
    public override void Accept(IActionVisitor visitor) => visitor.Visit(this);
}