using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

/// <summary>
/// A wrapped list of IModifierProviders that in turn implements IModifierProvider.
/// </summary>
public record ModifierCollection(ImmutableList<IModifierProvider> Modifiers) : IModifierProvider
{
    public static readonly ModifierCollection Empty = new([]);

    /// <summary>
    /// Returns a new ModifierCollection with the added modifier.
    /// </summary>
    /// <param name="modifier">Modifier to append to the collection.</param>
    /// <returns>A new ModifierCollection.</returns>
    public ModifierCollection With(IModifierProvider modifier)
    {
        return new ModifierCollection([.. Modifiers, modifier]);
    }

    /// <summary>
    /// Returns a new ModifierCollection with the added modifiers.
    /// </summary>
    /// <param name="modifiers">Modifiers to append to the collection.</param>
    /// <returns>A new ModifierCollection.</returns>
    public ModifierCollection With(IEnumerable<IModifierProvider> modifiers)
    {
        return new ModifierCollection([.. Modifiers.Concat(modifiers)]);
    }

    public ModifierStack GetContributingModifiers(IQuery query, BattleEnvironment env, ModifierContext ctx)
    {
        return Modifiers.Aggregate(new ModifierStack(),
            (stack, mod) => stack + mod.GetContributingModifiers(query, env, ctx));
    }
}