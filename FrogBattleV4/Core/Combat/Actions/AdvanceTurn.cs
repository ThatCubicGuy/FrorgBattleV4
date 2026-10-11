namespace FrogBattleV4.Core.Combat.Actions;

public class AdvanceTurn : ShardAction<TurnAdvanceData>
{
    public override void Accept(IActionVisitor visitor) => visitor.Visit(this);
}