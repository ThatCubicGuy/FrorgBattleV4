#nullable enable
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.EffectSystem.StatusEffects;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.EffectSystem;

public struct EffectInstanceContext
{
    [NotNull] public required string EffectId { get; init; }
    [NotNull] public required ISupportsEffects Holder { get; init; }
    public BattleMember? Target { get; init; }
    public BattleMember? EffectSource { get; init; }
    public required uint Stacks { get; init; }
    public required IEnumerable<IModifierRule> Modifiers { get; init; }
    public required IEnumerable<IMutatorComponent>? Mutators { get; init; }
}