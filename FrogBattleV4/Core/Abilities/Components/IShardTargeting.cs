using System.Collections.Immutable;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Abilities.Components;

public interface IShardTargeting
{
    [Pure]
    ImmutableList<AbilityTargetingContext> SelectTargets(LinkResolutionState state, BattleEnvironment env);
}