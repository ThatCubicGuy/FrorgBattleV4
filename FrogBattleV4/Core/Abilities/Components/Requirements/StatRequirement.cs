using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Components.Requirements;

public class StatRequirement : IShardRequirement
{
    public required IShard ParentShard { get; init; }
    public required StatId Stat { get; init; }
    public double? MinValue { get; init; } = null;
    public double? MaxValue { get; init; } = null;

    public virtual bool IsFulfilled(LinkResolutionState state, BattleEnvironment env)
    {
        return new StatQuery
        {
            Stat = Stat,
            Subject = state.User,
        }.Calculate(env).IsWithinRange(MinValue, MaxValue);
    }

    public void GenerateFulfill(LinkResolutionState state, BattleEnvironment env, LinkResolutionBuilder builder)
    {
    }
}