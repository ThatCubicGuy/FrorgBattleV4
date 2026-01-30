using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Components;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.Pipelines;
public static class StatPipeline
{
    public static double ComputePipeline(this StatCalcContext ctx)
    {
        if (ctx.Owner is not ISupportsEffects owner) return ctx.Owner.BaseStats.GetValueOrDefault(ctx.Stat, 0);
        var finalCtx = owner.AttachedEffects
            .Select(x => (x.Modifiers.OfType<IStatModifier>(), x.GetStacks(new EffectContext
            {
                Holder = owner,
                Target = ctx.Target as IBattleMember
            })))
            .Aggregate(ctx, (statCtx, tuple) => tuple.Item1
                .Aggregate(statCtx, (cur, mod) =>
                {
                    // A bit of a silly way to do stack calculations but idk how else
                    for (var i = 0; i < tuple.Item2; i++)
                    {
                        cur = mod.Apply(cur);
                    }

                    return cur;
                }));
        if (!ctx.Owner.BaseStats.TryGetValue(ctx.Stat, out var finalAmount)) return 0;
        
        return finalCtx.Mods.Apply(ctx.Owner.BaseStats[ctx.Stat], finalAmount);
    }
}