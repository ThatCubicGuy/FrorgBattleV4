namespace FrogBattleV4.Core.EffectSystem.Components;

public interface IModifierComponent
{
    string Stat { get; }
    double Amount { get; }
    Operator Operator { get; }
}