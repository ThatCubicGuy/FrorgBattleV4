using System.Diagnostics;
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.EffectSystem.Components;

public interface IModifierComponent
{
    ModifierStack GetContribution(IRelationshipContext ctx);
    string GetDescription();
}

public interface IModifierComponent<in TContext> : IModifierComponent where TContext : struct, IRelationshipContext
{
    /// <summary>
    /// Gets the contribution of this modifier in the current context.
    /// </summary>
    /// <param name="ctx">The context in which to calculate the modifier.</param>
    /// <returns>The modifier contribution.</returns>
    ModifierStack GetContribution(TContext ctx);

    ModifierStack IModifierComponent.GetContribution(IRelationshipContext ctx)
    {
        if (ctx is TContext calcContext) return GetContribution(calcContext);
        Debug.WriteLine(
            $"WARN: Cannot process context of type {ctx.GetType().Name}" +
            $" from modifier of type {typeof(TContext).Name}.");
        return new ModifierStack();
    }
}