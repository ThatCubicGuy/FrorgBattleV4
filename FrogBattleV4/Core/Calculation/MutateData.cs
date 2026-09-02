namespace FrogBattleV4.Core.Calculation;

public record MutateData(
    PoolId TargetPool,
    PoolMutChannel Channel);

public enum PoolMutChannel
{
    Cost,
    Regen
}