using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Requirements;

public class PoolRequirement : IShardRequirement
{
    public required PoolName Pool { get; init; }
    public double? MinValue { get; init; }
    public double? MaxValue { get; init; }

    public bool IsFulfilled(LinkResolutionState state, BattleEnvironment env)
    {
        return env.GetPoolValue(state.User, Pool).IsWithinRange(MinValue, MaxValue);
    }

    public void GenerateFulfill(LinkResolutionState state, BattleEnvironment env, CommandBuilder builder)
    {
        throw new System.NotImplementedException();
    }
}