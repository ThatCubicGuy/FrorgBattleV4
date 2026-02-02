#nullable enable
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.EffectSystem.Components;

public abstract class BasicModifierComponent<TContext> : IModifierComponent<TContext>
    where TContext : struct, IRelationshipContext
{
    public required ModifierOperation Operation { get; init; }
    public required double Amount { get; init; }
    public string? Name { get; init; } = null;
    public string? Description { get; init; } = null;
    /// <summary>
    /// Determines whether this modifier should apply in this context.
    /// </summary>
    /// <param name="ctx">The context in which to determine application.</param>
    /// <returns>True if the modifier can apply, false otherwise.</returns>
    public abstract bool AppliesInContext(TContext ctx);

    public ModifierStack GetContribution(TContext ctx)
    {
        return AppliesInContext(ctx) ? new ModifierStack().AddTo(Operation, Amount) : new ModifierStack();
    }

    public string GetDescription()
    {
        return Localization.GetTranslationFormatted(Description, Amount, Operation);
    }

    public override string ToString()
    {
        return Operation switch
        {
            ModifierOperation.AddValue => $"{(Amount > 0 ? '+' : string.Empty)}{Amount:N0} ",
            ModifierOperation.AddBasePercent => $"{(Amount > 0 ? '+' : string.Empty)}{Amount:P0} ",
            ModifierOperation.MultiplyTotal => $"{Amount:0.##}x ",
            _ => $"{Amount:F1}"
        };
    }
}