namespace FrogBattleV4.Core.EffectSystem.Components;

public interface IModifierComponent
{
    bool Modifies(string key, EffectContext? ctx = null);
    /// <summary>
    /// Applies the modifier in the current context. Returns the new value, not the delta.
    /// </summary>
    /// <param name="currentValue">The value calculated so far among a list of modifiers.</param>
    /// <param name="ctx">The context in which to calculate the modifier.</param>
    /// <returns>The modified value.</returns>
    double Apply(double currentValue, EffectContext ctx);
}