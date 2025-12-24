using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;

public interface IStatModifier : IModifierComponent
{
    string Stat { get; }

    /// <summary>
    /// Applies the stat modifier in the current context. Returns the new value, not the delta.
    /// </summary>
    /// <param name="ctx">The context in which to calculate the modifier.</param>
    /// <returns>The modified stat value.</returns>
    StatCalcContext Apply(StatCalcContext ctx);
}