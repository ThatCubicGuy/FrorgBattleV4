#nullable enable
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.EffectSystem;

public interface ISupportsEffects
{
    EffectContainer Effects { get; }
}