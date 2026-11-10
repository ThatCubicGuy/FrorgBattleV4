using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities.Reactions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "trigger")]
[JsonDerivedType(typeof(DamageTakenReaction), "damage_taken")]
[JsonDerivedType(typeof(PoolMutatedReaction), "pool_mutated")]
[JsonDerivedType(typeof(EffectAppliedReaction), "effect_applied")]
[JsonDerivedType(typeof(CounterTriggeredReaction), "counter_triggered")]
public interface IReaction
{
    void Accept(IReactionVisitor visitor);
}

public interface IReactionVisitor
{
    void Visit(DamageTakenReaction reaction);
    void Visit(PoolMutatedReaction reaction);
    void Visit(EffectAppliedReaction reaction);
    void Visit(CounterTriggeredReaction reaction);
}