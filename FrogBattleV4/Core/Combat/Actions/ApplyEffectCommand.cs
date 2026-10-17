using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Combat.Actions;

/// <summary>
/// Represents a command for applying an effect to a target.
/// </summary>
public class ApplyEffectCommand : BattleCommand<ApplyEffectData>
{
    public required double ApplicationChance { get; set; }
    public override void Accept(IActionVisitor visitor) => visitor.Visit(this);
}