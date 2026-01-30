namespace FrogBattleV4.Core.EffectSystem.Components;

public class PoolCostModifier : IModifierComponent
{
    public ModifierOperation Operation { get; init; }
    public double Amount { get; init; }
    public string PoolKey { get; init; }
}