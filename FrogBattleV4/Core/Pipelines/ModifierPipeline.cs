#nullable enable
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.Contexts;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.EffectSystem;

namespace FrogBattleV4.Core.Pipelines;

public static class ModifiersPipeline
{
    public static ModifierStack ComputeModifiers<TQuery>(this TQuery query, ModifiersCalcContext ctx)
        where TQuery : struct
    {
        var mods = new ModifierStack();
        var effCtx = new EffectInfoContext
        {
            Holder = ctx.Actor,
            Other = ctx.Other,
        };

        mods = ctx.Actor?.AttachedEffects.Aggregate(mods,
            (stack, effect) => stack + effect.GetContributions(effCtx, query)) ?? mods;

        if (query is DamageQuery damageQuery)
        {
            mods = ctx.Target?.DamageModifiers.Where(dm => dm.AppliesFor(damageQuery))
                .Aggregate(mods, (stack, modifier) => modifier.ModifierStack + stack) ?? mods;
        }

        mods = ctx.Ability?.Modifiers.Aggregate(mods,
            (stack, component) => stack + component.GetContributions(effCtx, query)) ?? mods;

        return mods;
    }
}

public readonly struct ModifiersCalcContext : IRelationshipContext
{
    public BattleMember? Actor { get; init; }
    public BattleMember? Other { get; init; }
    public ITargetable? Target { get; init; }
    public AbilityDefinition? Ability { get; init; }
}