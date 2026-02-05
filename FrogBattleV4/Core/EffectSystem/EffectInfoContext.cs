#nullable enable
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.EffectSystem;

public readonly struct EffectInfoContext : IRelationshipContext
{
    [NotNull] public required ISupportsEffects Holder { get; init; }
    public BattleMember? Other { get; init; }
    BattleMember? IActorContext.Actor => Holder as BattleMember;
}