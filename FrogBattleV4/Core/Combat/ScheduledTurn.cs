namespace FrogBattleV4.Core.Combat;

// TODO: Add the possibility of follow ups? I'm not sure if this should depend on the scheduler tbh
public record ScheduledTurn(EntityUid Actor, double BaseActionValue);