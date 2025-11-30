using System.Collections.Generic;

namespace FrogBattleV4.Core.EffectSystem;

public interface ISupportsEffects
{
    List<IEffect> ActiveEffects { get; }
}