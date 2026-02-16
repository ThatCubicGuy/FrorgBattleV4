#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.AbilitySystem.Components.Attacks;

public class AttackComponent : IAttackComponent
{
    public required StatId Scalar { get; init; }
    public required double Ratio { get; init; }
    public required DamageType Type { get; init; }
    public required AttackProperties AttackProperties { get; init; }

    /// <summary>
    /// A number between 0 and 1 that determines damage falloff for subsequent targets hit by a blast attack.
    /// </summary>
    public double Falloff { get; init; } = 0;

    public double? HitRate { get; init; }
    public ITargetingComponent? Targeting { get; init; }

    [Pure]
    public IEnumerable<DamageIntent> GetDamageRequests(AbilityExecContext ctx)
    {
        return (Targeting ?? ctx.Definition.Targeting)
            .SelectTargets(ctx)
            .Select(atc => new DamageIntent
            {
                BaseAmount = Ratio * Math.Pow(1 - Falloff, atc.Rank) *
                             new ModifierContext
                             {
                                 Actor = ctx.User,
                                 Other = atc.Target,
                                 Ability = ctx.Definition,
                                 Aiming = atc.Aiming,
                             }.ComputeStat(Scalar),
                Target = atc.Target as IDamageable,
                Type = Type,
                Targeting = atc.Aiming,
                CanCrit = AttackProperties.CanCrit,
            });
    }
}