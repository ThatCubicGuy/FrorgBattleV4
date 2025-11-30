using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem;

public class StatusEffectInstance : IEffect
{
    public List<ISubeffectComponent> SubEffects { get; init; }
}