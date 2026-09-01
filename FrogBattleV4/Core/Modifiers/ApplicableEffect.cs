using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Modifiers;

/// <summary>
/// Encapsulates an effect that may be applied to a battle member.
/// This effect may have context-dependent stacks. 
/// </summary>
public abstract class ApplicableEffect : IModifierProvider
{
    protected abstract ModifierRuleCollection ModifierRuleCollection { get; }

    [Pure]
    protected abstract int GetStacks(RelationContext ctx);

    [Pure]
    public ModifierStack GetContributingModifiers(Query query, ModifierContext ctx)
    {
        return ModifierRuleCollection.GetContributingModifiers(query, ctx) * GetStacks(query.Context);
    }
}