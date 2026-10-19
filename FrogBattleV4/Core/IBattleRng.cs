using System.Collections.Generic;

namespace FrogBattleV4.Core;

public interface IBattleRng
{
    /// <summary>
    /// Decides whether an event with chance <paramref name="chance"/> happens.
    /// </summary>
    /// <param name="chance">Chance for the event to happen.</param>
    /// <returns>True if a randomly generated number is below <paramref name="chance"/>, false otherwise.</returns>
    bool Chance(double chance);

    /// <summary>
    /// Chooses one random value from the given collection.
    /// </summary>
    /// <param name="values">Collection to select a value from.</param>
    /// <typeparam name="T">Type of the items from the collection.</typeparam>
    /// <returns>A single value from <paramref name="values"/>.</returns>
    T Choose<T>(IEnumerable<T> values);

    /// <summary>
    /// Shuffles the given enumerable in a random order.
    /// </summary>
    /// <param name="source">Source enumerable to shuffle.</param>
    /// <typeparam name="T">Type of the elements in the collection.</typeparam>
    /// <returns>A collection containing items from <paramref name="source"/> in a random order.</returns>
    IEnumerable<T> Shuffle<T>(IEnumerable<T> source);
}