using System.ComponentModel;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Abilities.Components;

namespace FrogBattleV4.Core.Abilities;

/// <summary>
/// Represents a single component of a link.
/// </summary>
public abstract class Shard : IShard
{
    #region Metadata

    /// <summary>
    /// A catchy name for this shard.
    /// </summary>
    [Localizable(true)]
    public required string Name { get; init; }

    /// <summary>
    /// Description of what this shard does.
    /// </summary>
    [Localizable(true)]
    public required string Description { get; init; }

    /// <summary>
    /// Describes the primary function of a shard.
    /// </summary>
    public required ShardFunction Function { get; init; }

    #endregion

    public required IShardRequirement Condition { get; init; }

    /// <inheritdoc/>
    public bool IsUsable(LinkResolutionState state, BattleEnvironment env) => Condition.IsFulfilled(state, env);

    /// <inheritdoc/>
    public abstract void Resolve(ref LinkResolutionState state, BattleEnvironment env, CommandBuilder builder);
}

/// <summary>
/// A part of a shard link, exposing methods for usability and resolution.
/// </summary>
public interface IShard
{
    /// <summary>
    /// Determines whether the shard can be used in the current circumstances.
    /// </summary>
    /// <param name="state">Current link resolution state.</param>
    /// <param name="env">Current environment.</param>
    /// <returns>True if the shard can be used, false otherwise.</returns>
    [Pure]
    bool IsUsable(LinkResolutionState state, BattleEnvironment env);

    void Resolve(ref LinkResolutionState state, BattleEnvironment env, CommandBuilder builder);
}

public enum ShardFunction
{
    /// <summary>
    /// Attack shards target enemies and deal damage.
    /// </summary>
    Attack,

    /// <summary>
    /// Impair shards target enemies and apply debuffs.
    /// </summary>
    Impair,

    /// <summary>
    /// Restore shards target allies and restore HP, Energy, or others.
    /// </summary>
    Restore,

    /// <summary>
    /// Support shards target allies and apply buffs.
    /// </summary>
    Support,

    /// <summary>
    /// Target shards transform the current link's target selection.
    /// </summary>
    Target,

    /// <summary>
    /// Enhance shards boost the capabilities of other shards in the link.
    /// </summary>
    Enhance,
}