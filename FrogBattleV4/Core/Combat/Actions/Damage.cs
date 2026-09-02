using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Combat.Actions;

/// <summary>
/// Represents a command for dealing damage to a target.
/// </summary>
public class Damage : ShardAction<DamageData>
{
    public required double TotalAmount { get; init; }
    public required CritResolution CritData { get; init; }
    public override void Accept(IActionVisitor visitor) => visitor.Visit(this);
}