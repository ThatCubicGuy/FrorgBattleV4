using System.Collections.Generic;

namespace FrogBattleV4.Core.EffectSystem;

public interface ISupportsEffects
{
    List<IAttributeModifier> ActiveEffects { get; }
}