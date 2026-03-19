#nullable enable
using System.Collections.Generic;

namespace FrogBattleV4.Core.Combat;

public struct BattleContext
{
    public required BattleManager Manager { get; init; }
    public required IBattleMember ActiveMember { get; init; }
    public IEnumerable<IBattleMember>? Allies { get; init; }
    public IEnumerable<IBattleMember>? Enemies { get; init; }
    public long TurnNumber { get; init; }
    public required System.Random Rng { get; init; }
}