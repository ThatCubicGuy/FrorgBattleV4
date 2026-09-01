using System.Collections.Generic;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Combat.Selections;

public record AbilitySelectionRequest(GameEntity Requestor, IEnumerable<AbilityShard> ValidOptions, int Count = 1)
    : ISelectionRequest<AbilityShard>;

public record TargetSelectionRequest(GameEntity Requestor, IEnumerable<GameEntity> ValidOptions, int Count = 1)
    : ISelectionRequest<GameEntity>;