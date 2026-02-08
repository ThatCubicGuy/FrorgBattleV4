#nullable enable
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.CharacterSystem;

public struct StatCalcContext : IRelationshipContext
{
    [NotNull] public required string Stat { get; init; }
    [NotNull] public required BattleMember Actor { get; init; }
    public BattleMember? Other { get; init; }
}