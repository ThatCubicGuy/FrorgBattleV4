#nullable enable
namespace FrogBattleV4.Core.EffectSystem.Components;

public class TypeResPen : IPenaltyModifier
{
    public required double Amount { get; init; }
    public string? Type { get; init; }
}