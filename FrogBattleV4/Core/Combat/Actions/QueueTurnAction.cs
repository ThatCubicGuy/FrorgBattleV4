namespace FrogBattleV4.Core.Combat.Actions;

public class QueueTurnAction : ShardAction<QueueTurnData>
{
    public override void Accept(IActionVisitor visitor) => visitor.Visit(this);
}