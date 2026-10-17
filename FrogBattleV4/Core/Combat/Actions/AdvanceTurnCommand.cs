namespace FrogBattleV4.Core.Combat.Actions;

public class AdvanceTurnCommand : BattleCommand<TurnAdvanceData>
{
    public override void Accept(IActionVisitor visitor) => visitor.Visit(this);
}