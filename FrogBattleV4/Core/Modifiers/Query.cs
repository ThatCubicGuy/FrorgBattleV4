using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Effects;

public abstract record Query
{
    /// <summary>
    /// Base value to start calculations from.
    /// </summary>
    public required double BaseValue { get; init; }
}

/// <summary>
/// Base query for things that create an action;
/// mostly for mutations.
/// </summary>
public abstract record RelationQuery : Query
{
    /// <summary>
    /// Context in which to query.
    /// </summary>
    public required RelationContext Context { get; init; }
    public abstract void Accept(IRelationQueryVisitor visitor);
}

/// <summary>
/// Base query for values that are inherent to one entity;
/// 
/// </summary>
public abstract record StaticQuery : Query
{
    public required EntityId Subject { get; init; }
    public EntityId? Reference { get; init; }
}