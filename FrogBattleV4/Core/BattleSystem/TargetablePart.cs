using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.BattleSystem;

public class TargetablePart : ITargetable
{
    public required BattleMember Parent { get; init; }
    public required IEnumerable<TargetTag> Tags { get; init; }
    public IEnumerable<DamageModifier> DamageModifiers { get; init; } = [];
}