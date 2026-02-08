using System;
using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.EffectSystem;

public interface ISupportsEffects
{
    IEnumerable<IModifierComponent> AttachedEffects { get; }
    bool ApplyEffect(StatusEffectApplicationContext ctx);
    bool RemoveEffect(StatusEffectRemovalContext ctx);
    event EventHandler<StatusEffectApplicationContext> EffectApplySuccess;
    event EventHandler<StatusEffectApplicationContext> EffectApplyFailure;
    event EventHandler<StatusEffectRemovalContext> EffectRemoveSuccess;
    event EventHandler<StatusEffectRemovalContext> EffectRemoveFailure;
}