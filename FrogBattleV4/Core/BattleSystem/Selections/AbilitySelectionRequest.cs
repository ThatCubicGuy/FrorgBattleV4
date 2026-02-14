using System.Collections.Generic;
using FrogBattleV4.Core.AbilitySystem;

namespace FrogBattleV4.Core.BattleSystem.Selections;

public record AbilitySelectionRequest(BattleMember Requestor, IEnumerable<AbilityDefinition> ValidOptions, int Count = 1)
    : ISelectionRequest<AbilityDefinition>;

public record TargetSelectionRequest(BattleMember Requestor, IEnumerable<BattleMember> ValidOptions, int Count = 1)
    : ISelectionRequest<BattleMember>;

public record PartSelectionRequest(BattleMember Requestor, IEnumerable<ITargetable> ValidOptions, int Count = 1)
    : ISelectionRequest<ITargetable>;