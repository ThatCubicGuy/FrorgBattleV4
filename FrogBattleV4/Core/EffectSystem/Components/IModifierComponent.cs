using System.Diagnostics;

namespace FrogBattleV4.Core.EffectSystem.Components;

public interface IModifierComponent
{
    ModifierStack GetContribution(object ctx);
    string GetDescription();
}

public interface IModifierComponent<in TContext> : IModifierComponent where TContext : struct
{
    /// <summary>
    /// Gets the contribution of this modifier in the current context.
    /// </summary>
    /// <param name="ctx">The context in which to calculate the modifier.</param>
    /// <returns>The modifier contribution.</returns>
    ModifierStack GetContribution(TContext ctx);

    ModifierStack IModifierComponent.GetContribution(object ctx)
    {
        if (ctx is TContext calcContext) return GetContribution(calcContext);
        Debug.WriteLine(
            $"INFO: Cannot process context of type {ctx.GetType().Name}" +
            $" from modifier of type {typeof(TContext).Name}.");
        return default;
    }
}