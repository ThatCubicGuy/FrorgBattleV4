#nullable enable

namespace FrogBattleV4.Core.Contexts;

public record RelationshipWrapper(BattleMember? Actor, BattleMember? Other) : IRelationshipContext;