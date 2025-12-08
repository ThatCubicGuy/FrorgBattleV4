namespace FrogBattleV4.Core.EffectSystem.Components;

public interface IModifierComponent
{
    ModifierOperation Operation { get; }
    double Amount { get; }
}