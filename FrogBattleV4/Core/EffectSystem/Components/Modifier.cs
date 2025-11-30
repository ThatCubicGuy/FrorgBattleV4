using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class Modifier : IModifierComponent
{
    public string Stat { get; init; }
    public double Amount { get; init; }
    public Operator Operator { get; init; }

    public double Apply(ICharacter holder)
    {
        return Operator switch
        {
            Operator.AddValue => Amount,
            Operator.MultiplyBase => Amount * holder.BaseStats[Stat],
            Operator.MultiplyTotal => throw new System.NotSupportedException($"Operation {Operator.MultiplyTotal} not supported"),
            _ => throw new System.InvalidOperationException($"Invalid operator: {Operator}")
        };
    }
    public string GetKey() => string.Join('.', nameof(Modifier), Stat);
}