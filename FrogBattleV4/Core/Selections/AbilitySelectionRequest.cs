using System.Collections.Immutable;
using FrogBattleV4.Core.Abilities;

namespace FrogBattleV4.Core.Selections;

public record AbilitySelectionRequest(EntityUid Requestor, ImmutableList<IShard> ValidOptions)
    : ISelectionRequest<IShard>;

public record TargetSelectionRequest(EntityUid Requestor, ImmutableList<EntityUid> ValidOptions)
    : ISelectionRequest<EntityUid>;