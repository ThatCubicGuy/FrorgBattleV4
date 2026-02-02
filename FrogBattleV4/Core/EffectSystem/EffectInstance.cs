#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem;

public class EffectInstance
{
    public required string Id { get; init; }
    public uint Stacks { get; init; } = 1;
    public required IEnumerable<IModifierComponent>? Modifiers { get; init; }
}