#nullable enable
namespace FrogBattleV4.Core.EffectSystem.Components;

public class SimpleStatModifier : IModifierComponent
{
    public required string Stat { get; init; }
    public required double Amount { get; init; }
    public required Operator Operator { get; init; }

    public bool Modifies(string key, EffectContext? ctx = null) => Stat == key;

    public double Apply(double currentValue, EffectContext ctx)
    {
        return Operator switch
        {
            Operator.AddValue => currentValue + Amount,
            Operator.MultiplyBase => currentValue + Amount * ctx.Holder.BaseStats[Stat],
            Operator.MultiplyTotal => currentValue * Amount,
            _ => throw new System.InvalidOperationException($"Invalid operator: {Operator}")
        };
    }
}