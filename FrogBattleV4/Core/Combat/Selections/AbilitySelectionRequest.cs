using System.Collections.Generic;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Combat.Selections;

public record AbilitySelectionRequest(IBattleMember Requestor, IEnumerable<AbilityDefinition> ValidOptions, int Count = 1)
    : ISelectionRequest<AbilityDefinition>;

public record TargetSelectionRequest(IBattleMember Requestor, IEnumerable<IBattleMember> ValidOptions, int Count = 1)
    : ISelectionRequest<IBattleMember>;