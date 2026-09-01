using FrogBattleV4.Core.Effects.Modifiers;

namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Mutation query classifying damage.
/// </summary>
public record DamageQuery(CalcDirection Direction, MutModifierDirection MutModifierDirection) : MutModifierQuery(Direction, MutModifierDirection)
{
    public DamageType Type { get; init; }
    public DamageSource Source { get; init; }
    public required bool Crit { get; init; }
}