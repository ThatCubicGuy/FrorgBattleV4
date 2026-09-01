using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Components.Requirements;

public class StatRequirement : IShardRequirement
{
    public required AbilityShard ParentShard { get; init; }
    public required StatId Stat { get; init; }
    public double? MinValue { get; init; } = null;
    public double? MaxValue { get; init; } = null;

    public virtual bool IsFulfilled(ShardLinkContext ctx)
    {
        return new InteractionContext
        {
            Actor = ctx.User,
            Other = ctx.SelectedTarget,
            Ability = ParentShard,
            Rng = ctx.Rng,
        }.ComputeStat(Stat).IsWithinRange(MinValue, MaxValue);
    }
}