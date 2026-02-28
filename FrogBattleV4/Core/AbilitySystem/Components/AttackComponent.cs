#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components;

public class AttackComponent : IAbilityCommandComponent
{
    public required StatId Scalar { get; init; }
    public required double Ratio { get; init; }
    public required DamageType Type { get; init; }
    public required AttackProperties AttackProperties { get; init; }

    /// <summary>
    /// A number between 0 and 1 that determines damage falloff for subsequent targets hit by a blast attack.
    /// </summary>
    public double Falloff { get; init; }

    public double? HitRate { get; init; }
    public ITargetingComponent? Targeting { get; init; }

    public IEnumerable<IBattleCommand> GetContribution(AbilityExecContext ctx)
    {
        return ((Targeting ?? ctx.Definition.Targeting)
            .SelectTargets(ctx)
            .Select(atc => new DamageCommand
            {
                BaseAmount = Ratio * Math.Pow(1 - Falloff, atc.Rank) *
                             new ModifierContext
                             {
                                 Actor = ctx.User,
                                 Other = atc.Target,
                                 Ability = ctx.Definition,
                                 Aiming = atc.Aiming,
                             }.ComputeStat(Scalar),
                Target = atc.Target,
                Type = Type,
                Targeting = atc.Aiming,
                CanCrit = AttackProperties.CanCrit,
            }));
    }
}

/// <summary>
/// Lightweight record for storing properties related to attacks.
/// </summary>
/// <param name="CanCrit">Whether this attack can generate critical hits.</param>
public record AttackProperties(bool CanCrit);