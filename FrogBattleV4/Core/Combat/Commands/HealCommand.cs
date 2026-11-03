using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Combat.Commands;

public class HealCommand : BattleCommand<HealData>
{
    public required double Amount;
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
}