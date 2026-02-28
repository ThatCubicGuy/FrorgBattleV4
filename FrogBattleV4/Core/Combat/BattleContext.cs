#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.Combat.Actions;

namespace FrogBattleV4.Core.Combat;

public struct BattleContext
{
    public required BattleManager Manager { get; init; }
    public IBattleMember ActiveMember { get; init; }
    public IEnumerable<IBattleMember>? Allies { get; init; }
    public IEnumerable<IBattleMember>? Enemies { get; init; }
    public uint TurnNumber { get; init; }
    public IOrderedEnumerable<ActionBarItem> ActionOrder { get; init; }
    public required Random Rng { get; init; }
}