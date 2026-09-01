using FrogBattleV4.Core.Abilities;

namespace FrogBattleV4.Core.Combat.Actions;

public record QueueActionCommand(IScheduledAction Action) : ShardCommand;