using System.Collections.Immutable;
using FrogBattleV4.Core.Abilities;

namespace FrogBattleV4.Core.Combat;

/// <summary>
/// Represents a decision made during a turn in order to execute a shard link on a target.
/// </summary>
/// <param name="Link">Shard link to execute.</param>
/// <param name="MainTarget">Main target to select.</param>
public sealed record TurnDecision(ImmutableArray<ShardId> Link, EntityUid MainTarget);