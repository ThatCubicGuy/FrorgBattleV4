using System.Collections.Generic;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Combat;

public struct BattleContext
{
    public required BattleManager Manager { get; init; }
    public required GameEntity ActiveMember { get; init; }
    public IEnumerable<GameEntity>? Allies { get; init; }
    public IEnumerable<GameEntity>? Enemies { get; init; }
    public long TurnNumber { get; init; }
    public required System.Random Rng { get; init; }
}