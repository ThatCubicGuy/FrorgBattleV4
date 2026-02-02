#nullable enable
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class SimpleStatModifier : BasicModifierComponent<StatCalcContext>
{
    public required string Stat { get; init; }
    public override bool AppliesInContext(StatCalcContext ctx)
    {
        return ctx.Stat == Stat;
    }

    public override string ToString()
    {
        return base.ToString() + Stat;
    }
}