using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities;

/// <summary>
/// Represents a single component of a link.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ShardClasses.TargetShard), "target")]
[JsonDerivedType(typeof(ShardClasses.EmpowerShard), "empower")]
[JsonDerivedType(typeof(ShardClasses.AffectShard), "affect")]
public abstract class Shard
{
    public ImmutableArray<IShardRequirement> Requirements { get; init; } = [];

    /// <summary>
    /// Determines whether the shard can be used in the current circumstances.
    /// </summary>
    /// <param name="state">Current link resolution state.</param>
    /// <param name="env">Current environment.</param>
    /// <returns>True if the shard can be used, false otherwise.</returns>
    [Pure]
    public bool IsUsable(LinkResolutionState state, BattleEnvironment env) =>
        Requirements.All(sr => sr.IsFulfilled(state, env));

    /// <summary>
    /// Resolves the shard in the current state and environment.
    /// </summary>
    /// <param name="state">Link resolution state to resolve in, passed by reference.</param>
    /// <param name="env">Environment to resolve in.</param>
    /// <param name="builder">Command builder for the shard commands to be launched.</param>
    /// <param name="rng">Battle RNG for resolving randomness requests.</param>
    public abstract void Resolve(ref LinkResolutionState state, BattleEnvironment env, CommandBuilder builder,
        IBattleRng rng);
}