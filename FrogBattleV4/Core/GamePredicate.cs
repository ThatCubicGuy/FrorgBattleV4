namespace FrogBattleV4.Core;

/// <summary>
/// An object that determines whether an instance of type <typeparamref name="T"/> matches certain conditions.
/// </summary>
/// <typeparam name="T">Type of elements to evaluate against.</typeparam>
public abstract class GamePredicate<T>
{
    public abstract bool Evaluate(T value);
}

/// <summary>
/// Provides methods to safely and concisely evaluate nullable predicates.
/// </summary>
public static class GamePredicate
{
    /// <summary>
    /// Safely evaluates a nullable predicate and returns true if it is null.
    /// </summary>
    /// <param name="predicate">Predicate to evaluate. Can be null, in which case this method returns true.</param>
    /// <param name="value">Value to evaluate against.</param>
    /// <typeparam name="T">Type of values the predicate supports.</typeparam>
    /// <returns>True if the predicate is null -or- the predicate was evaluated successfully.</returns>
    public static bool SafeEvaluate<T>(this GamePredicate<T>? predicate, T value)
    {
        return predicate is null || predicate.Evaluate(value);
    }
}