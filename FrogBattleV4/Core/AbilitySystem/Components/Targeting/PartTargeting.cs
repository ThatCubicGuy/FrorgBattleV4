#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

internal static class PartTargeting
{
    public static IEnumerable<TargetingContext> Slam(
        BattleMember target,
        int startingRank,
        bool reversed = false)
    {
        var result = target.Parts.Cast<ITargetable?>();
        if (reversed) result = result.Reverse();
        return result.Where(part => part is not null).Select((part, index) => new TargetingContext
        {
            Target = part,
            TargetRank = index + startingRank,
        });
    }

    public static TargetingContext ByTag(
        BattleMember target,
        int startingRank,
        TargetTag tag)
    {
        return new TargetingContext
        {
            Target = target.Parts.Cast<ITargetable?>().SingleOrDefault(part => part?.Tags.Contains(tag) ?? false),
            TargetRank = startingRank,
        };
    }
    
    
    public static IEnumerable<TargetingContext> FullyCustom(
        BattleMember target,
        int startingRank,
        int mainTargetHeight,
        int radius,
        int pierce,
        bool heightLosesRank,
        bool pierceLosesRank)
    {
        var result = new List<TargetingContext>();
        for (var i = Math.Max(0, mainTargetHeight - radius); i < Math.Min(target.Parts.Depth, mainTargetHeight + radius); i++)
        {
            
        }
    }

    private static ITargetable? NthAtHeight(
        BattleMember target,
        int height,
        int n = 0)
    {
        for (var i = new TargetPosition(height); i.Depth < target.Parts.Depth; i = new TargetPosition(height, i.Depth + 1))
        {
            if (target.Parts[i] is not null && --n < 0) return target.Parts[i];
        }

        return null;
    }
}