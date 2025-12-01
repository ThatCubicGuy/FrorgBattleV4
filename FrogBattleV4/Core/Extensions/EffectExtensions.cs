using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.Extensions;

public static class EffectExtensions
{
    public static double Apply(this IModifierComponent x, double currentValue, EffectContext ctx)
    {
        return x.Operator switch
        {
            Operator.AddValue => x.Amount,
            Operator.MultiplyBase => x.Amount * ctx.Holder.BaseStats[x.Stat],
            Operator.MultiplyTotal => x.Amount * currentValue,
            _ => throw new System.InvalidOperationException($"Invalid operator: {x.Operator}")
        };
    }
}