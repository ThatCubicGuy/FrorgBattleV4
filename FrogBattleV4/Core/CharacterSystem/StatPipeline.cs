using System.Linq;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.CharacterSystem;

public static class StatPipeline
{
    public static double ComputePipeline(this StatContext ctx)
    {
        return ctx.Owner.AttachedEffects.Sum(mod => mod.Modifiers.OfType<IStatModifier>()
            .Where(x => x.Stat == ctx.Stat)
            .Aggregate(ctx.Owner.BaseStats[ctx.Stat], (current, modifier) =>
                modifier.Apply(current, new EffectContext
                {
                    EffectSource = (mod as ActiveEffectInstance)?.Source,
                    Holder = ctx.Owner,
                    Target = ctx.Target
                })));
    }
}