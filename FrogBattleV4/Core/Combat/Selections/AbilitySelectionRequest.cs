using System.Collections.Generic;
using FrogBattleV4.Core.AbilitySystem;

namespace FrogBattleV4.Core.BattleSystem.Selections;

public record AbilitySelectionRequest(IBattleMember Requestor, IEnumerable<AbilityDefinition> ValidOptions, int Count = 1)
    : ISelectionRequest<AbilityDefinition>;

public record TargetSelectionRequest(IBattleMember Requestor, IEnumerable<IBattleMember> ValidOptions, int Count = 1)
    : ISelectionRequest<IBattleMember>;

public record PartSelectionRequest(IBattleMember Requestor, IEnumerable<ITargetable> ValidOptions, int Count = 1)
    : ISelectionRequest<ITargetable>;