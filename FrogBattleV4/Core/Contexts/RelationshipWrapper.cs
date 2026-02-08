#nullable enable
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.Contexts;

public record RelationshipWrapper(BattleMember? Actor, BattleMember? Other) : IRelationshipContext;