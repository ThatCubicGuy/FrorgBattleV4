#nullable enable
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.EffectSystem;

public readonly struct EffectInfoContext : IRelationshipContext
{
    public BattleMember? Actor { get; init; }
    public BattleMember? Other { get; init; }
}