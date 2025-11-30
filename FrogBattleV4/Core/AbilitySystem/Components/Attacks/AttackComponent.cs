#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Attacks;

public class AttackComponent : IAttackComponent
{
    public required string Scalar { get; init; }
    public required double Ratio { get; init; }
    public double? HitRate { get; init; }
    public ITargetingComponent? Targeting { get; init; }

    public IEnumerable<Damage> GetDamage(AbilityContext ctx)
    {
        Damage[] result = [];
        foreach (var item in (Targeting ?? ctx.Definition.Targeting)?.SelectTargets(ctx) ?? [])
        {
            result[^1] = new Damage(ctx.User, item);
        }
        return result;
    }
}