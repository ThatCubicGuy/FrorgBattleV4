using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Components.Actions;

/// <summary>
/// Represents a command for dealing damage to a target.
/// </summary>
public class DealDamage : ShardAction<DamageData>
{
    public required double TotalAmount { get; init; }
    public required bool Crit { get; init; }
    public override void Accept(IActionVisitor visitor) => visitor.Visit(this);
}