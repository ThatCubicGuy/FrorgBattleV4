namespace FrogBattleV4.Core.EffectSystem.Components;

public class DefensePen : IPenaltyModifier
{
    public required ModifierOperation Operation { get; init; } = ModifierOperation.MultiplyTotal;
    public required double Amount { get; init; }
}