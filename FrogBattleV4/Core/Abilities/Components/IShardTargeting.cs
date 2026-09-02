using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Abilities.Components;

public interface IShardTargeting
{
    [Pure]
    TargetingSelection SelectTargets(LinkResolutionState state, BattleEnvironment env);
}