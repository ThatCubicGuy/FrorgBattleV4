namespace FrogBattleV4.Core.Combat.Commands;

public class AdvanceTurnCommand : BattleCommand<TurnAdvanceData>
{
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
}