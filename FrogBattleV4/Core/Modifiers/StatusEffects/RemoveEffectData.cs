namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public record RemoveEffectData(
    StatusEffectPredicate Predicate,
    int RemovedStacks = 1,
    int RemovedTurns = 0);