namespace FrogBattleV4.Core.Abilities;

/// <summary>
/// Small scope object for a single shard's resolution.
/// </summary>
/// <param name="User">User of the shard.</param>
/// <param name="Targeting">Target of the shard.</param>
public record ShardResolutionScope(EntityUid User, TargetingContext Targeting);