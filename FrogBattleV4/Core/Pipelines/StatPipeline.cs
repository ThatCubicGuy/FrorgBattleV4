using System.Collections.Generic;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.Pipelines;
public static class StatPipeline
{
    public static double ComputePipeline(this StatCalcContext ctx)
    {
        if (ctx.Owner is not ISupportsEffects owner) return ctx.Owner.BaseStats.GetValueOrDefault(ctx.Stat, 0);
        var finalMods = owner.AttachedEffects
            .AggregateMods(ctx, new EffectContext
            {
                Holder = owner,
                Target = ctx.Target as IBattleMember,
            });
        
        return ctx.Owner.BaseStats.TryGetValue(ctx.Stat, out var baseStatValue) ?
            finalMods.Apply(baseStatValue) : 0;
    }
}