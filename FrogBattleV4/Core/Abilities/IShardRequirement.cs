using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Requirements.AllRequirement), "all_of")]
[JsonDerivedType(typeof(Requirements.AnyRequirement), "any_of")]
[JsonDerivedType(typeof(Requirements.StatRequirement), "stat")]
[JsonDerivedType(typeof(Requirements.PoolRequirement), "pool")]
public interface IShardRequirement
{
    /// <summary>
    /// Determines whether the requirement is fulfilled in this context.
    /// </summary>
    /// <param name="state">Data about the shard link so far.</param>
    /// <param name="env">Battle environment to check in.</param>
    /// <returns>True if the request is satisfied, false otherwise.</returns>
    [Pure]
    bool IsFulfilled(LinkResolutionState state, BattleEnvironment env);
    /// <summary>
    /// Adds whatever actions are needed to fulfill the requirement.
    /// </summary>
    /// <param name="state">Data about the shard link so far.</param>
    /// <param name="env">Battle environment to check in.</param>
    /// <param name="builder">Resolution builder to add actions to.</param>
    void GenerateFulfill(LinkResolutionState state, BattleEnvironment env, CommandBuilder builder);
}