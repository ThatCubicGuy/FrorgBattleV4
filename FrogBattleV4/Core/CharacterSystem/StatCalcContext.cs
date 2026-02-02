#nullable enable
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.CharacterSystem;

public struct StatCalcContext : IRelationshipContext
{
    public required string Stat { get; init; }
    public required BattleMember Actor { get; init; }
    public BattleMember? Other { get; init; }
}