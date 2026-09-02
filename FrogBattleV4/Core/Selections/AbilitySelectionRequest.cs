using System.Collections.Generic;
using System.Collections.Immutable;
using FrogBattleV4.Core.Abilities;

namespace FrogBattleV4.Core.Selections;

public record AbilitySelectionRequest(EntityUid Requestor, ImmutableList<IShard> ValidOptions, int Count = 1)
    : ISelectionRequest<IShard>
{
    IReadOnlyList<IShard> ISelectionRequest<IShard>.ValidOptions => ValidOptions;
}

public record TargetSelectionRequest(EntityUid Requestor, ImmutableList<EntityUid> ValidOptions, int Count = 1)
    : ISelectionRequest<EntityUid>
{
    IReadOnlyList<EntityUid> ISelectionRequest<EntityUid>.ValidOptions => ValidOptions;
}