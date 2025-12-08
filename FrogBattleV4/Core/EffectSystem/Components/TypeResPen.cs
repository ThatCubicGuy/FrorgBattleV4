#nullable enable
namespace FrogBattleV4.Core.EffectSystem.Components;

public class TypeResPen : IPenaltyModifier
{
    public required ModifierOperation Operation { get; init; } = ModifierOperation.MultiplyTotal;
    public required double Amount { get; init; }
    public string? Type { get; init; }
}