using System;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public record RemoveEffectData(
    Func<StatusEffectInstance, bool> Predicate,
    int RemovedStacks = 1,
    int RemovedTurns = 0);