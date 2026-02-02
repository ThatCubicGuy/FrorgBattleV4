using System;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.BattleSystem.Decisions;

public interface IDecisionRequest<out TResult> : IDecisionRequest
{
    IReadOnlyList<TResult> ValidOptions { get; }
}

public interface IDecisionRequest
{
    BattleMember Requestor { get; }
    int Count { get; }
}

public static class DecisionRequestExtensions
{
    public static DecisionResult<TResult> Select<TResult>(this IDecisionRequest<TResult> request, int selection)
    {
        return request.Select(new Range(selection, selection + 1));
    }
    public static DecisionResult<TResult> Select<TResult>(this IDecisionRequest<TResult> request, int start, int end)
    {
        return request.Select(new Range(start, end));
    }
    public static DecisionResult<TResult> Select<TResult>(this IDecisionRequest<TResult> request, Range range)
    {
        return new DecisionResult<TResult>(request, request.ValidOptions.Take(range).ToList());
    }
}