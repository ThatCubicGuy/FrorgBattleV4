using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Combat.Commands;

/// <summary>
/// Represents a command for applying an effect to a target.
/// </summary>
public class ApplyEffectCommand : BattleCommand<ApplyEffectData>
{
    public required double ApplicationChance { get; set; }
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
}