namespace FrogBattleV4.Core.Calculation;

public interface IQuery
{
    EntityUid Main { get; }
    EntityUid? Other { get; }
}

/// <summary>
/// Base query for things that create an action, and exist in a relationship;
/// Values that need some context, including a base value, because they only exist once created.
/// </summary>
public abstract record DynamicQuery : IQuery
{
    /// <summary>
    /// Base value to start calculations from.
    /// </summary>
    public required double BaseValue { get; init; }

    /// <summary>
    /// Context in which to query.
    /// </summary>
    public required RelationContext Context { get; init; }

    EntityUid IQuery.Main => Context.Actor;
    EntityUid? IQuery.Other => Context.Target;
}

/// <summary>
/// Base query for values that are inherent to an entity;
/// Values that don't need a base value because they always exist in this way.
/// </summary>
public abstract record StaticQuery : IQuery
{
    public required EntityUid Subject { get; init; }
    public EntityUid? Reference { get; init; }
    EntityUid IQuery.Main => Subject;
    EntityUid? IQuery.Other => Reference;
}