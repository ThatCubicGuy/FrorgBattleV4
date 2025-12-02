namespace FrogBattleV4.Core.EffectSystem.Components;

public interface IStatModifier : IModifierComponent
{
    string Stat { get; }
    ModifierOperation Operation { get; }
    /// <summary>
    /// Applies the stat modifier in the current context. Returns the new value, not the delta.
    /// </summary>
    /// <param name="currentValue">The value calculated so far among a list of modifiers.</param>
    /// <param name="ctx">The context in which to calculate the modifier.</param>
    /// <returns>The modified stat value.</returns>
    double Apply(double currentValue, StatContext ctx);
}