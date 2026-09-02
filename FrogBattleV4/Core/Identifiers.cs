using System;

namespace FrogBattleV4.Core;

public abstract class Identifiable<TUid> where TUid : struct, IUniqueIdentifier<TUid>
{
    public TUid Id { get; } = new();
    public override string ToString() => $"ID #{Id}";
    public sealed override int GetHashCode() => Id.GetHashCode();
    public sealed override bool Equals(object? obj) => obj is Identifiable<TUid> id && Id.Equals(id.Id);
}

public interface IUniqueIdentifier<T> : IEquatable<T> where T : struct, IUniqueIdentifier<T>;

public readonly record struct EntityUid() : IUniqueIdentifier<EntityUid>
{
    private static long _idCounter;
    private long Id { get; } = ++_idCounter;
}

public readonly record struct TeamUid() : IUniqueIdentifier<TeamUid>
{
    private static long _idCounter;
    private long Id { get; } = ++_idCounter;
}