using System.Collections.Generic;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.BattleSystem.Selections;

public record AbilitySelectionRequest(IBattleMember Requestor, IEnumerable<AbilityDefinition> ValidOptions, int Count = 1)
    : ISelectionRequest<AbilityDefinition>;

public record TargetSelectionRequest(IBattleMember Requestor, IEnumerable<IDamageable> ValidOptions, int Count = 1)
    : ISelectionRequest<IDamageable>;