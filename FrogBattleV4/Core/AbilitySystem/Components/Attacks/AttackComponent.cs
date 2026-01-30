#nullable enable
using System;
using System.Collections.Generic;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Attacks;

public class AttackComponent : IAttackComponent
{
    public required string Scalar { get; init; }
    public required double Ratio { get; init; }
    public required DamageProperties Properties { get; init; }
    public double? Falloff { get; init; }
    public double? HitRate { get; init; }
    public ITargetingComponent? Targeting { get; init; }

    public IEnumerable<Damage> GetDamage(AbilityExecContext ctx)
    {
        IList<Damage> result = [];
        foreach (var target in (Targeting ?? ctx.Definition.Targeting)!.SelectTargets(ctx))
        {
            var ratio = Ratio * Math.Pow(1 - (Falloff ?? 0), target.TargetRank);
            result.Add(new Damage
            {
                BaseAmount = ratio * ctx.User.GetStat(Scalar, target.Target.Owner),
                Properties = Properties,
                Source = ctx.User,
                Target = target.Target,
                Random = ctx.Rng
            });
        }
        return result;
    }
}