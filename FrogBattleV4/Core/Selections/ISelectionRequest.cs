using System.Collections.Immutable;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Selections;

public interface ISelectionRequest<TResult>
{
    IImmutableList<TResult> ValidOptions { get; }
}

public static class SelectionRequestExtensions
{
    [Pure]
    public static SelectionResult<TResult> Select<TResult>(this ISelectionRequest<TResult> request,
        params int[] selections)
    {
        return new SelectionResult<TResult>(request, selections);
    }
}