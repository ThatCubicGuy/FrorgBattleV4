#nullable enable
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Effects.Modifiers;

namespace FrogBattleV4.Core.Effects;

/// <summary>
/// Encapsulates an effect that may be applied to a battle member.
/// This effect may have context-dependent stacks. 
/// </summary>
public abstract class ApplicableEffect : IModifierProvider
{
    protected abstract ModifierCollection ModifierCollection { get; }

    [Pure]
    protected abstract int GetStacks(ModifierContext ctx);

    [Pure]
    public ModifierStack GetContributingModifiers<TQuery>(ModifierQuery<TQuery> query, ModifierContext ctx)
        where TQuery : struct => ModifierCollection.GetContribution(query) * GetStacks(ctx);
}