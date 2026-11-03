namespace FrogBattleV4.Core;

/// <summary>
/// An object that determines whether an instance of type <typeparamref name="T"/> matches certain conditions.
/// </summary>
/// <typeparam name="T">Type of elements to evaluate against.</typeparam>
public abstract class GamePredicate<T>
{
    public abstract bool Evaluate(T value);
}