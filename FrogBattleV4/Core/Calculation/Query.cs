namespace FrogBattleV4.Core.Calculation;

public interface IQuery
{
    EntityUid Main { get; }
    EntityUid? Other { get; }
}

/// <summary>
/// Base query for values that are inherent to an entity;
/// Values that don't need a base value because they always exist in this way.
/// </summary>
public abstract class AttributeQuery : IQuery
{
    public required EntityUid Subject { get; init; }
    public EntityUid? Reference { get; init; }

    /// <summary>
    /// Returns this query's subject.
    /// </summary>
    EntityUid IQuery.Main => Subject;

    /// <summary>
    /// Returns this query's reference.
    /// </summary>
    EntityUid? IQuery.Other => Reference;
}

/// <summary>
/// Base query for things that exist in a relationship, at a certain point in time;
/// Values that need some context, including a base value, because they only exist once created.
/// </summary>
public abstract class CommandQuery : IQuery
{
    /// <summary>
    /// Base value to start calculations from.
    /// </summary>
    public required double BaseValue { get; init; }

    /// <summary>
    /// Context in which the query happens.
    /// </summary>
    public required RelationContext Context { get; init; }

    /// <summary>
    /// Returns the context's actor.
    /// </summary>
    EntityUid IQuery.Main => Context.Actor;

    /// <summary>
    /// Returns the context's target.
    /// </summary>
    EntityUid? IQuery.Other => Context.Target;
}