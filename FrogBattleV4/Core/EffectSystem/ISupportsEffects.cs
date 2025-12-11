using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;

namespace FrogBattleV4.Core.EffectSystem;

public interface ISupportsEffects
{
    IReadOnlyList<IAttributeModifier> AttachedEffects { get; }
    void ApplyEffect(EffectApplicationContext effect);
    bool RemoveEffect(ActiveEffectDefinition effect);
}