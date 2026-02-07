using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace FrogBattleV4.Core.BattleSystem.Selections;

public interface ISelectionRequest<out TResult> : ISelectionRequest
{
    IEnumerable<TResult> ValidOptions { get; }
}

public interface ISelectionRequest
{
    IBattleMember Requestor { get; }
    int Count { get; }
}

public static class DecisionRequestExtensions
{
    [Pure]
    public static SelectionResult<TResult> Select<TResult>(this ISelectionRequest<TResult> request, int selection)
    {
        return request.Select(new Range(selection, selection + 1));
    }

    [Pure]
    public static SelectionResult<TResult> Select<TResult>(this ISelectionRequest<TResult> request, int start, int end)
    {
        return request.Select(new Range(start, end));
    }

    [Pure]
    public static SelectionResult<TResult> Select<TResult>(this ISelectionRequest<TResult> request, Range range)
    {
        return new SelectionResult<TResult>(request.ValidOptions.Take(range));
    }
}