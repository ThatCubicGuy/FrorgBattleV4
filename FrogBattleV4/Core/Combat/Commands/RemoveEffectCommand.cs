using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Combat.Commands;

public class RemoveEffectCommand : BattleCommand<RemoveEffectData>
{
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
}