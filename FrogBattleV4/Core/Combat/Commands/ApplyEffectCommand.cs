using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Combat.Commands;

/// <summary>
/// Represents a command for applying an effect to a target.
/// </summary>
public class ApplyEffectCommand : BattleCommand
{
    public required double ApplicationChance { get; init; }
    public required ApplyEffectData Data { get; init; }
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
}