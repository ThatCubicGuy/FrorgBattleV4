using System.Collections.Immutable;
using FrogBattleV4.Core.Abilities;

namespace FrogBattleV4.Core.Selections;

public record AbilitySelectionRequest(EntityUid Requestor, IImmutableList<IShard> ValidOptions, int Count = 1)
    : ISelectionRequest<IShard>;

public record TargetSelectionRequest(EntityUid Requestor, IImmutableList<EntityUid> ValidOptions, int Count = 1)
    : ISelectionRequest<EntityUid>;