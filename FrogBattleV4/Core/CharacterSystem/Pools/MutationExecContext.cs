#nullable enable
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.CharacterSystem.Pools;

/// <summary>
/// Stores the context in which any pool mutation will happen.
/// </summary>
public readonly struct MutationExecContext(IHasPools holder) : IRelationshipContext
{
    [NotNull] public required IHasPools Holder { get; init; } = holder;
    public BattleMember? Other { get; init; }
    BattleMember? IActorContext.Actor => Holder as BattleMember;
}