using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Requirements;

public class StatRequirement : IShardRequirement
{
    public required StatName Stat { get; init; }
    public double? MinValue { get; init; }
    public double? MaxValue { get; init; }

    public virtual bool IsFulfilled(LinkResolutionState state, BattleEnvironment env)
    {
        return new StatQuery
        {
            Stat = Stat,
            Subject = state.User,
        }.Calculate(env).IsWithinRange(MinValue, MaxValue);
    }

    public void GenerateFulfill(LinkResolutionState state, BattleEnvironment env, CommandBuilder builder)
    {
    }
}