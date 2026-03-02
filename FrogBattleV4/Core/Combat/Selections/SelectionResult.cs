namespace FrogBattleV4.Core.Combat.Selections;

// Work on this or don't, IDK.
public interface ISelectionResult<out TResult>
{
    TResult[] Choices { get; }
}

public record SelectionResult<TResult>(TResult[] Choices) : ISelectionResult<TResult>;