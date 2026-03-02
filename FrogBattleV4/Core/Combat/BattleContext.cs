#nullable enable
using System;
using System.Collections.Generic;

namespace FrogBattleV4.Core.Combat;

public struct BattleContext
{
    public IBattleMember ActiveMember { get; init; }
    public IEnumerable<IBattleMember>? Allies { get; init; }
    public IEnumerable<IBattleMember>? Enemies { get; init; }
    public long TurnNumber { get; init; }
    public required Random Rng { get; init; }
}