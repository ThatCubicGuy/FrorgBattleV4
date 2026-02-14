#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.BattleSystem.Actions;

namespace FrogBattleV4.Core.BattleSystem;

public struct BattleContext
{
    public required BattleManager Manager { get; init; }
    public BattleMember ActiveMember { get; init; }
    public IEnumerable<BattleMember>? Allies { get; init; }
    public IEnumerable<BattleMember>? Enemies { get; init; }
    public uint TurnNumber { get; init; }
    public IOrderedEnumerable<ActionBarItem> ActionOrder { get; init; }
    public required Random Rng { get; init; }
}