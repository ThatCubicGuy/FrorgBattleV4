#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Attacks;

public class AttackComponent : IAttackComponent
{
    public required StatId Scalar { get; init; }
    public required double Ratio { get; init; }
    public required DamageProperties DamageProperties { get; init; }
    public required AttackProperties AttackProperties { get; init; }
    /// <summary>
    /// A number between 0 and 1 that determines damage falloff for subsequent targets hit by a blast attack.
    /// </summary>
    public double Falloff { get; init; } = 0;
    public double? HitRate { get; init; }
    public ITargetingComponent? Targeting { get; init; }

    [Pure]
    public IEnumerable<DamageRequest> GetDamageRequests(AbilityExecContext ctx)
    {
        return (Targeting ?? ctx.Definition.Targeting)!
            .SelectTargets(ctx)
            .Select(tc => new DamageRequest
            {
                BaseAmount = Ratio * Math.Pow(1 - Falloff, tc.TargetRank) *
                             ctx.User.GetStat(Scalar, tc.Target.Parent),
                Properties = DamageProperties,
                Target = tc.Target.Parent as IDamageable,
                ExtraModifiers = tc.Target.DamageModifiers
            });
    }
}