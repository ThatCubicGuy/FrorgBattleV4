using System.ComponentModel;
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

    public required UsageClass ShardFunction { get; init; }

    /// <summary>
    /// Defines the primary function of this shard.
    /// </summary>
    public enum UsageClass
    {
        /// <summary>
        /// Attack shards target enemy targets and deal damage.
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
        /// Enhance shards boost the capabilities of other shards in the chain.
        /// </summary>
        Enhance,
    }

    #endregion

    public required IShardRequirement Condition { get; init; }

    public bool IsUsable(LinkResolutionState state, BattleEnvironment env) => Condition.IsFulfilled(state, env);
    public abstract void Resolve(ref LinkResolutionState state, BattleEnvironment env, LinkResolutionBuilder builder);
}

/// <summary>
/// A part of a shard link, exposing methods for usability and resolution.
/// </summary>
public interface IShard
{
    bool IsUsable(LinkResolutionState state, BattleEnvironment env);
    void Resolve(ref LinkResolutionState state, BattleEnvironment env, LinkResolutionBuilder builder);
}