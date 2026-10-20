using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Combat.Commands;

/// <summary>
/// Represents a command for dealing damage to a target.
/// </summary>
public class DamageCommand : BattleCommand<DamageData>
{
    public required double NormalAmount { get; init; }
    public required double CriticalAmount { get; init; }
    public required double CriticalChance { get; init; }
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
}