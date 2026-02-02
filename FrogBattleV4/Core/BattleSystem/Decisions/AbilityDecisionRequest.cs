using System.Collections.Generic;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.BattleSystem.Decisions;

public record AbilityDecisionRequest(BattleMember Requestor, IReadOnlyList<AbilityDefinition> ValidOptions, int Count = 1)
    : IDecisionRequest<AbilityDefinition>;

public record TargetDecisionRequest(BattleMember Requestor, IReadOnlyList<IDamageable> ValidOptions, int Count = 1)
    : IDecisionRequest<IDamageable>;