using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Targeting;

public class BounceTargeting : IShardTargeting
{
    public required int Count { get; set; }
    public int RankPenaltyPerBounce { get; set; } = 0;
    public IEnumerable<AbilityTargetingContext> SelectTargets(ShardLinkContext ctx)
    {
        var targets = new List<AbilityTargetingContext>();
        for (var i = 0; i < Count; i++)
        {
            var next = ctx.State.AlliedTeamOf(ctx.SelectedTarget).Members.MinBy(_ => ctx.Rng.NextDouble());
            if (next is not null) targets.Add(new AbilityTargetingContext
            {
                Target = next,
                Rank = i * RankPenaltyPerBounce,
            });
        }

        return targets.Prepend(new AbilityTargetingContext
        {
            Target = ctx.SelectedTarget,
            Rank = 0,
        });
    }
}