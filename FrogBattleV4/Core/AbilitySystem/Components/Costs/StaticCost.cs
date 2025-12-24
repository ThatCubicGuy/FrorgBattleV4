#nullable enable
namespace FrogBattleV4.Core.AbilitySystem.Components.Costs;

public class StaticCost : ICostComponent
{
    public required string Stat { get; init; }
    public required double Amount { get; init; }

    public void Tax(AbilityExecContext ctx)
    {
        throw new System.NotImplementedException();
    }
}