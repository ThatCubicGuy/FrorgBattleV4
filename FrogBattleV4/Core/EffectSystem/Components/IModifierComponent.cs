namespace FrogBattleV4.Core.EffectSystem.Components;

public interface IModifierComponent
{
    double Amount { get; }
    string GetKey();
}