using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Components.Actions;

/// <summary>
/// Represents a command for applying an effect to a target.
/// </summary>
public class ApplyEffect : ShardAction<ApplyEffectData>
{
    public override void Accept(IActionVisitor visitor) => visitor.Visit(this);
}