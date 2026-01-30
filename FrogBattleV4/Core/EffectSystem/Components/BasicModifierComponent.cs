namespace FrogBattleV4.Core.EffectSystem.Components;

public abstract class BasicModifierComponent<TContext> : IModifierComponent<TContext>, IBasicModifierComponent where TContext : struct
{
    public required ModifierOperation Operation { get; init; }
    public required double Amount { get; init; }
    /// <summary>
    /// Determines whether this modifier should apply in this context.
    /// </summary>
    /// <param name="ctx">The context in which to determine application.</param>
    /// <returns>True if the modifier can apply, false otherwise.</returns>
    public abstract bool AppliesInContext(TContext ctx);

    public ModifierStack GetContribution(TContext ctx)
    {
        return AppliesInContext(ctx) ? default(ModifierStack).AddTo(Operation, Amount) : default;
    }

    public string GetDescription()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Formats the relevant arguments into a format string.
    /// Argument order:
    /// {0}: <see cref="Amount"/>,
    /// {1}: <see cref="Operation"/>.
    /// </summary>
    /// <param name="format">Format string in which to place the arguments.</param>
    /// <returns>The formatted string.</returns>
    public string Format(string format)
    {
        return string.Format(format, Amount, Operation);
    }
}

internal interface IBasicModifierComponent
{
    double Amount { get; }
    ModifierOperation Operation { get; }
}