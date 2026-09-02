using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

/// <summary>
/// A wrapped list of IModifierProviders that in turn implements IModifierProvider.
/// </summary>
public record ModifierCollection : IModifierProvider
{
    public static readonly ModifierCollection Empty = new([]);

    public ModifierCollection(IEnumerable<IModifierProvider> modifiers)
    {
        Modifiers = modifiers.ToImmutableList();
    }

    public ImmutableList<IModifierProvider> Modifiers { get; init; }

    public ModifierCollection With(IModifierProvider modifier)
    {
        return new ModifierCollection(Modifiers.Append(modifier));
    }

    public ModifierCollection With(IEnumerable<IModifierProvider> modifiers)
    {
        return new ModifierCollection(Modifiers.Concat(modifiers));
    }

    public ModifierStack GetContributingModifiers(IQuery query, BattleEnvironment env, ModifierContext ctx)
    {
        return Modifiers.Aggregate(new ModifierStack(),
            (stack, mod) => stack + mod.GetContributingModifiers(query, env, ctx));
    }
}