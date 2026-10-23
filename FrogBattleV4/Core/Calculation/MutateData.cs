namespace FrogBattleV4.Core.Calculation;

public record MutateData(
    PoolName TargetPool,
    PoolMutChannel Channel);

public enum PoolMutChannel
{
    Cost,
    Regen
}