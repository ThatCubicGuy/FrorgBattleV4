using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Components.Actions;

public class RemoveEffect : ShardAction<RemoveEffectData>
{
    public override void Accept(IActionVisitor visitor) => visitor.Visit(this);
}