using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Modifiers;

/// <summary>
/// Encapsulates an effect that may be applied to a battle member.
/// This effect may have context-dependent stacks. 
/// </summary>
public abstract class ApplicableEffect : IModifierProvider
{
    public IEnumerable<ModifierRule> ModifierRules { get; init; } = [];
    public IEnumerable<IMutatorComponent> Mutators { get; init; } = [];

    [Pure]
    protected abstract int GetStacks(EntityUid subject, EntityUid? reference, BattleEnvironment env);

    [Pure]
    public abstract bool ShouldRemove();

    [Pure]
    public ModifierStack GetContributingModifiers(IQuery query, BattleEnvironment env, ModifierContext ctx)
    {
        return ModifierRules.Where(mr => mr.Applies(query, ctx))
            .Aggregate(new ModifierStack(), (stack, rule) =>
                stack + rule.ModifierStack) * GetStacks(query.Main, query.Other, env);
    }
}