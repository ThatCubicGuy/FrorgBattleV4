using FrogBattleV4.Core.Abilities;

namespace FrogBattleV4.Core.Combat.Actions;

public record ActionAdvanceCommand(IBattleMember Target, double AdvancePercent) : IBattleCommand;