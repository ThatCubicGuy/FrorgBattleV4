using System.Collections.Generic;

namespace FrogBattleV4.Core.BattleSystem.Decisions;

public interface IDecisionResult
{
    IDecisionRequest Request { get; }
}

public interface IDecisionResult<T> : IDecisionResult
{
    new IDecisionRequest<T> Request { get; }
    IReadOnlyList<T> Choices { get; }
}

public class DecisionResult<T>(IDecisionRequest<T> request, IReadOnlyList<T> choices) : IDecisionResult
{
    public IDecisionRequest Request { get; } = request;
    public IReadOnlyList<T> Choices { get; } = choices;
}