#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

internal static class PartTargeting
{
    public static IEnumerable<TargetingContext> Vertical(
        BattleMember target,
        int startingRank,
        bool fromTop = false)
    {
        var result = target.Parts.OfType<ITargetable>();
        if (fromTop) result = result.Reverse();
        return result.Select((part, index) => new TargetingContext
        {
            Target = part,
            TargetRank = index + startingRank,
        });
    }

    public static IEnumerable<TargetingContext> ByTag(
        BattleMember target,
        int startingRank,
        TargetIdentifier id,
        bool rankDecay = false)
    {
        var i = 0;
        return target.Parts
            .Where(part => id.Equals(part?.Identifier))
            .Select(part =>
                new TargetingContext
                {
                    Target = part,
                    TargetRank = startingRank + (rankDecay ? i++ : i),
                });
    }

    public static IEnumerable<TargetingContext> ByHeight(
        BattleMember target,
        int startingRank,
        int height)
    {
        yield return new TargetingContext
        {
            Target = target.Parts[height],
            TargetRank = startingRank,
        };
    }

    public static IEnumerable<TargetingContext> AllParts(
        BattleMember target,
        int startingRank) =>
        CustomBlast(target, startingRank, 0, 100, false);

    private static List<TargetingContext> CustomBlast(
        BattleMember target,
        int startingRank,
        int mainTargetIndex,
        int radius,
        bool heightRankDecay)
    {
        var result = new List<TargetingContext>();
        // Inclusive lower bound
        var lowerBound = Math.Max(0, mainTargetIndex - radius);
        // Exclusive upper bound
        var upperBound = Math.Min(target.Parts.Height, mainTargetIndex + radius + 1);
        for (var i = lowerBound;
             i < upperBound;
             i++)
        {
            result.Add(new TargetingContext
            {
                Target = target.Parts[i],
                TargetRank = startingRank + (heightRankDecay ? Math.Abs(i - mainTargetIndex) : 0),
            });
        }

        return result;
    }
}