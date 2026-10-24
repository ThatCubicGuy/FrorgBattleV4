using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

/// <summary>
/// A wrapped list of IModifierRules and IModifierProviders that in turn implements IModifierProvider.
/// </summary>
public record ModifierCollection(ImmutableList<IModifierRule> Rules, ImmutableList<IModifierProvider> Providers) : IModifierProvider
{
    public static readonly ModifierCollection Empty = new(
        ImmutableList<IModifierRule>.Empty,
        ImmutableList<IModifierProvider>.Empty);

    /// <summary>
    /// Returns a new ModifierCollection with the added modifier.
    /// </summary>
    /// <param name="modifier">Modifier to append to the collection.</param>
    /// <returns>A new ModifierCollection.</returns>
    public ModifierCollection With(IModifierProvider modifier)
    {
        return this with { Providers = Providers.Add(modifier) };
    }

    /// <summary>
    /// Returns a new ModifierCollection with the added modifiers.
    /// </summary>
    /// <param name="modifiers">Modifiers to append to the collection.</param>
    /// <returns>A new ModifierCollection.</returns>
    public ModifierCollection With(IEnumerable<IModifierProvider> modifiers)
    {
        return this with { Providers = Providers.AddRange(modifiers) };
    }

    public ModifierCollection With(IModifierRule rule)
    {
        return this with { Rules = Rules.Add(rule) };
    }

    public ModifierCollection With(IEnumerable<IModifierRule> rules)
    {
        return this with { Rules = Rules.AddRange(rules) };
    }

    public ModifierStack GetContributingModifiers(IQuery query, BattleEnvironment env, ModifierContext ctx)
    {
        return Providers.Aggregate(new ModifierStack(),
            (stack, mod) => stack + mod.GetContributingModifiers(query, env, ctx)) +
               ModifierRules.Aggregate(Rules, query, ctx);
    }
}