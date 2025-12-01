namespace FrogBattleV4.Core.EffectSystem.Components;

public class Modifier : IModifierComponent
{
    public string Stat { get; init; }
    public double Amount { get; init; }
    public Operator Operator { get; init; }
}