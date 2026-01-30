#nullable enable
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class SimpleStatModifier : IStatModifier
{
    public required string Stat { get; init; }
    public required double Amount { get; init; }
    public required ModifierOperation Operation { get; init; }
    // public required IAttributeModifier Parent { get; init; }
    public StatCalcContext Apply(StatCalcContext ctx)
    {
        if (ctx.Stat != Stat) return ctx;
        ctx.Mods = ctx.Mods.Add(Operation, Amount);
        return ctx;
    }
}