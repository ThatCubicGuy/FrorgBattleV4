#nullable enable
namespace FrogBattleV4.Core.AbilitySystem.Components.Costs;

public class StaticCost : ICostComponent
{
    public required string Pool { get; init; }
    public required double BaseAmount { get; init; }

    public double GetBaseAmount(AbilityExecContext ctx) => BaseAmount;
}