using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Combat.Actions;

public record ActionAdvanceCommand(IBattleMember Target, double AdvancePercent);