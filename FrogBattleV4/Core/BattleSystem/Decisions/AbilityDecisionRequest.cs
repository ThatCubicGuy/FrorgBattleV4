using System.Collections.Generic;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem.Decisions;

public record AbilityDecisionRequest(IBattleMember Requestor, IReadOnlyList<AbilityDefinition> ValidOptions, int Count = 1)
    : IDecisionRequest<AbilityDefinition>;

public record TargetDecisionRequest(IBattleMember Requestor, IReadOnlyList<ITargetable> ValidOptions, int Count = 1)
    : IDecisionRequest<ITargetable>;