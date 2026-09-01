using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;

namespace FrogBattleV4.Core.Modifiers;

public class ModifierRuleCollection(params ModifierRule[] rules) :
    IReadOnlyCollection<ModifierRule>,
    IModifierProvider
{
    private readonly ImmutableList<ModifierRule> _rules = rules.ToImmutableList();

    public int Count => _rules.Count;

    /// <summary>
    /// Aggregates all contained modifiers that contribute to this query.
    /// </summary>
    /// <param name="query">Query to search for.</param>
    /// <param name="ctx">Modifier context to search in regard to.</param>
    /// <returns>The aggregated ModifierStack.</returns>
    [Pure]
    public ModifierStack GetContributingModifiers(Query query, ModifierContext ctx)
    {
        return _rules.Where(mr => mr.Applies(query, ctx))
            .Aggregate(new ModifierStack(), (stack, rule) =>
                stack + rule.ModifierStack);
    }

    public IEnumerator<ModifierRule> GetEnumerator() => _rules.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}