namespace FrogBattleV4.Core.Combat.Commands;

public class AdvanceTurnCommand : BattleCommand
{
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    public required double AdvancePercent { get; init; }
}