using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Combat.Actions;

public class RemoveEffect : ShardAction<RemoveEffectData>
{
    public override void Accept(IActionVisitor visitor) => visitor.Visit(this);
}