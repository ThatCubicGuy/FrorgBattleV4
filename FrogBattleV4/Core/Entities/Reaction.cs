using System;
using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Entities;

/// <summary>
/// A reaction is an event consumer that launches commands. (?)
/// </summary>
public abstract class Reaction
{
    // TODO (Nova)
    public abstract void Accept(IReactionVisitor visitor);
}

public class DamageTakenReaction : Reaction
{
    // Predicate
    public required DamageData Data { get; init; }
    public void HandleDamageTaken(object? sender, DamageTakenEventArgs e)
    {
        if (!e.Data.Matches(Data)) return;
    }

    public override void Accept(IReactionVisitor visitor) => visitor.Visit(this);
}

public class PoolValueChangedReaction : Reaction
{
    public void HandlePoolValueChanged(object? sender, PoolValueChangedEventArgs e)
    {

    }

    public override void Accept(IReactionVisitor visitor) => visitor.Visit(this);
}

public abstract class ReactionFunction
{
    
}

public class HealReactionFunction : ReactionFunction
{
    public required double Ratio { get; init; }
    [JsonPropertyName("entity")]
    public AffectedSide AffectedEntity { get; init; }
}

public interface IReactionVisitor
{
    void Visit(DamageTakenReaction reaction);
    void Visit(PoolValueChangedReaction reaction);
}