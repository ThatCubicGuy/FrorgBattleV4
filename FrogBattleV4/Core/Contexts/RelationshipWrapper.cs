#nullable enable
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.Contexts;

public record RelationshipWrapper(IBattleMember? Actor, IBattleMember? Other) : IRelationshipContext;