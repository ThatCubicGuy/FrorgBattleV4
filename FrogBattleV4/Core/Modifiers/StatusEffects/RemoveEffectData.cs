using System;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public record RemoveEffectData(
    Func<StatusEffectInstance, bool> Predicate,
    int RemovedStacks,
    int RemovedTurns);