using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities.Reactions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "trigger")]
[JsonDerivedType(typeof(DirectDamageTakenReaction), "damage_taken")]
[JsonDerivedType(typeof(DirectDamageDealtReaction), "damage_dealt")]
[JsonDerivedType(typeof(PoolMutatedReaction), "pool_mutated")]
[JsonDerivedType(typeof(CounterTriggeredReaction), "counter_triggered")]
[JsonDerivedType(typeof(TeammateAttackedReaction), "teammate_attacked")]
public interface IReaction
{
    void Accept(IReactionVisitor visitor);
}

public interface IReactionVisitor
{
    void Visit(DirectDamageTakenReaction reaction);
    void Visit(DirectDamageDealtReaction reaction);
    void Visit(PoolMutatedReaction reaction);
    void Visit(CounterTriggeredReaction reaction);
    void Visit(TeammateAttackedReaction reaction);
}