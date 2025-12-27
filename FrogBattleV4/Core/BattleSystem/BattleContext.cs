using System;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.BattleSystem;

public struct BattleContext
{
    public IReadOnlyCollection<IBattleMember> Allies { get; init; }
    public IReadOnlyCollection<IBattleMember> Enemies { get; init; }
    public uint TurnNumber { get; init; }
    public IOrderedEnumerable<IBattleMember> ActionOrder { get; init; }
    public required Random Rng { get; init; }
}