using System.Collections.Generic;
using FrogBattleV4.Core.Contexts;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;

namespace FrogBattleV4.Core.EffectSystem;

public interface ISupportsEffects
{
    IEnumerable<EffectInstance> GetAttachedEffects(IRelationshipContext ctx);
    bool ApplyEffect(ActiveEffectApplicationContext activeEffect);
    bool RemoveEffect(ActiveEffectRemovalContext ctx);
}