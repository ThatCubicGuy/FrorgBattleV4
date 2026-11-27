using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Combat.Commands;

public class RemoveEffectCommand : BattleCommand
{
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
    public required RemoveEffectData Data { get; init; }
}