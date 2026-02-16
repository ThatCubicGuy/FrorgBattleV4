using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.EffectSystem;

public class ModifierCollection([NotNull] params ModifierRule[] rules) : IEnumerable<ModifierRule>, IModifierProvider
{
    private readonly ImmutableList<ModifierRule> _rules = rules.ToImmutableList();
    
    [NotNull] public IReadOnlyCollection<ModifierRule> Rules => _rules;
    
    /// <summary>
    /// Aggregates all contained modifiers that contribute to this query.
    /// </summary>
    /// <param name="query">Query to search for.</param>
    /// <typeparam name="TRequest">Type of the request to process.</typeparam>
    /// <returns>The aggregated ModifierStack.</returns>
    [Pure]
    public ModifierStack GetContribution<TRequest>(ModifierQuery<TRequest> query) where TRequest : struct
    {
        return _rules.Where(mr => mr.AppliesTo(query))
            .Aggregate(new ModifierStack(), (stack, rule) =>
                stack + rule.ModifierStack);
    }

    ModifierStack IModifierProvider.GetContributingModifiers<TQuery>(ModifierQuery<TQuery> query, ModifierContext ctx)
        where TQuery : struct => GetContribution(query);

    public IEnumerator<ModifierRule> GetEnumerator() => _rules.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}