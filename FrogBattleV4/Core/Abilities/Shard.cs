using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities;

/// <summary>
/// Represents a single component of a link.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ShardClasses.TargetShard), "target")]
[JsonDerivedType(typeof(ShardClasses.EmpowerShard), "empower")]
[JsonDerivedType(typeof(ShardClasses.AffectShard), "affect")]
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

    public required IShardRequirement Requirement { get; init; }

    /// <inheritdoc/>
    public bool IsUsable(LinkResolutionState state, BattleEnvironment env) => Requirement.IsFulfilled(state, env);

    /// <inheritdoc/>
    public abstract void Resolve(ref LinkResolutionState state, BattleEnvironment env, CommandBuilder builder,
        IBattleRng rng);
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

    void Resolve(ref LinkResolutionState state, BattleEnvironment env, CommandBuilder builder, IBattleRng rng);
}

[JsonConverter(typeof(JsonStringEnumConverter<ShardFunction>))]
public enum ShardFunction
{
    /// <summary>
    /// Attack shards target enemies and deal damage.
    /// </summary>
    [JsonStringEnumMemberName("attack")]
    Attack,

    /// <summary>
    /// Impair shards target enemies and apply debuffs.
    /// </summary>
    [JsonStringEnumMemberName("impair")]
    Impair,

    /// <summary>
    /// Restore shards target allies and restore HP, Energy, or others.
    /// </summary>
    [JsonStringEnumMemberName("restore")]
    Restore,

    /// <summary>
    /// Support shards target allies and apply buffs.
    /// </summary>
    [JsonStringEnumMemberName("support")]
    Support,

    /// <summary>
    /// Target shards transform the current link's target selection.
    /// </summary>
    [JsonStringEnumMemberName("target")]
    Target,

    /// <summary>
    /// Enhance shards boost the capabilities of other shards in the link.
    /// </summary>
    [JsonStringEnumMemberName("enhance")]
    Enhance,
}