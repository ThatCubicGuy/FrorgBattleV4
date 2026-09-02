namespace FrogBattleV4.Core.Combat.Actions;

public class QueueTurn : ShardAction<QueueTurnData>
{
    public override void Accept(IActionVisitor visitor) => visitor.Visit(this);
}