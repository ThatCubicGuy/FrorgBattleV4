using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Combat.Actions;

public record ActionAdvanceCommand(GameEntity Target, double AdvancePercent) : ShardCommand;