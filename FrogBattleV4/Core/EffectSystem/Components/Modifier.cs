namespace FrogBattleV4.Core.EffectSystem.Components;

public class Modifier : ISubeffectComponent
{
    public string Stat { get; init; }
    public double Amount { get; init; }
    public Operators Operator { get; init; }

    public double Apply(EffectContext context)
    {
        return Operator switch
        {
            Operators.AddValue => Amount,
            Operators.MultiplyBase => Amount * context.Holder.BaseStats[Stat],
            Operators.MultiplyTotal => throw new System.NotSupportedException($"Operation {Operators.MultiplyTotal} not supported"),
            _ => throw new System.InvalidOperationException($"Invalid operator: {Operator}")
        };
    }
    public string GetKey() => string.Join('.', nameof(Modifier), Stat);
}