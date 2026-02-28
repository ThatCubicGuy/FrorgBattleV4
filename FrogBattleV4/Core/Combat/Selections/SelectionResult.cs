using System.Collections.Generic;

namespace FrogBattleV4.Core.Combat.Selections;

// Work on this or don't, IDK.
public interface ISelectionResult<out TResult>
{
    IEnumerable<TResult> Choices { get; }
}

public record SelectionResult<TResult>(IEnumerable<TResult> Choices) : ISelectionResult<TResult>;