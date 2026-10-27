using System;

namespace FrogBattleV4.Core;

public abstract class UniquelyIdentifiable<TUid> where TUid : struct, IUniqueIdentifier<TUid>
{
    public TUid Uid { get; } = new();
    public override string ToString() => $"ID #{Uid}";
    public sealed override int GetHashCode() => Uid.GetHashCode();
    public sealed override bool Equals(object? obj) => obj is UniquelyIdentifiable<TUid> id && Uid.Equals(id.Uid);
}

/// <summary>
/// An object that uniquely identifies an instance from a collection
/// of other instances identified by the same type of identifier.
/// </summary>
/// <typeparam name="TSelf">The type implementing this interface.</typeparam>
public interface IUniqueIdentifier<TSelf> : IEquatable<TSelf> where TSelf : struct, IUniqueIdentifier<TSelf>;

public readonly record struct EntityUid() : IUniqueIdentifier<EntityUid>
{
    private static long _idCounter;
    private long Id { get; } = checked(++_idCounter);
}

public readonly record struct TeamUid() : IUniqueIdentifier<TeamUid>
{
    private static long _idCounter;
    private long Id { get; } = checked(++_idCounter);
}