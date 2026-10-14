using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FrogBattleV4.Core.Selections;

public class SelectionResult<TResult>(IEnumerable<TResult> choices)
{
    public SelectionResult(ISelectionRequest<TResult> request, IEnumerable<int> selections) :
        this(selections.Select(x => request.ValidOptions[x]))
    {
    }

    public ImmutableList<TResult> Choices { get; } = choices.ToImmutableList();
}