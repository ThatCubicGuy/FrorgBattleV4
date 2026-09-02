using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FrogBattleV4.Core.Selections;

// Work on this or don't, IDK.
public interface ISelectionResult<out TResult>
{
    IReadOnlyCollection<TResult> Choices { get; }
}

public class SelectionResult<TResult>(IEnumerable<TResult> choices) : ISelectionResult<TResult>
{
    public SelectionResult(ISelectionRequest<TResult> request, IEnumerable<int> selections) : this(
        selections.Select(x => request.ValidOptions[x]))
    {
    }

    public ImmutableList<TResult> Choices { get; } = choices.ToImmutableList();
    IReadOnlyCollection<TResult> ISelectionResult<TResult>.Choices => Choices;
}