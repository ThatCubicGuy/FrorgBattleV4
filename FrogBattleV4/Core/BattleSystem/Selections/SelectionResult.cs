using System.Collections;
using System.Collections.Generic;

namespace FrogBattleV4.Core.BattleSystem.Selections;

public interface ISelectionResult
{
    IEnumerable Choices { get; }
}

// Work on this or don't, IDK.
public interface ISelectionResult<out TResult> : ISelectionResult
{
    new IEnumerable<TResult> Choices { get; }
    IEnumerable ISelectionResult.Choices => Choices;
}

public record SelectionResult<TResult>(IEnumerable<TResult> Choices) : ISelectionResult<TResult>;