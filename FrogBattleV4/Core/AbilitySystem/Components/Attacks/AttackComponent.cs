#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Attacks;

public class AttackComponent : IAttackComponent
{
    public required string Scalar { get; init; }
    public required double Ratio { get; init; }
    public required DamageProperties DamageProperties { get; init; }
    public required AttackProperties AttackProperties { get; init; }
    public double Falloff { get; init; } = 0;
    public double? HitRate { get; init; }
    public ITargetingComponent? Targeting { get; init; }

    [Pure]
    public IEnumerable<DamageRequest> GetDamageRequests(AbilityExecContext ctx)
    {
        return (Targeting ?? ctx.Definition.Targeting)!
            .SelectTargets(ctx)
            .Select(target => new DamageRequest
            {
                BaseAmount = Ratio * Math.Pow(1 - Falloff, target.TargetRank) *
                             ctx.User.GetStat(Scalar, target.Target.Parent),
                Properties = DamageProperties,
                Target = target.Target,
                Rng = ctx.Rng
            });
    }
}