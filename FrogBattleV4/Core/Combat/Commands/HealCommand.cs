using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Combat.Commands;

public class HealCommand : BattleCommand
{
    public required double Amount { get; init; }
    public required HealFlags Flags { get; init; }
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
}