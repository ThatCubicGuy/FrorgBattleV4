using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem;

public interface IEffect
{
    List<IModifierComponent> SubEffects { get; init; }
}