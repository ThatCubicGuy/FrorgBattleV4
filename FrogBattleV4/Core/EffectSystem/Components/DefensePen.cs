namespace FrogBattleV4.Core.EffectSystem.Components;

public class DefensePen : IPenaltyModifier
{
    public required double Amount { get; init; }
}