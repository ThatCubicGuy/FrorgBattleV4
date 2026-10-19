using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Combat.Commands;

/// <summary>
/// Represents a command for dealing damage to a target.
/// </summary>
public class DamageCommand : BattleCommand<DamageData>
{
    public required double TotalAmount { get; init; }
    public required bool IsCrit { get; init; }
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
}