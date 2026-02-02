#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem;

/// <summary>
/// An attribute modifier is a wrapper of various modifiers that is
/// attached to a fighter, and only makes sense in that context.
/// </summary>
public interface IAttributeModifier
{
    IReadOnlyList<IModifierComponent>? Modifiers { get; }
    uint GetStacks(EffectContext ctx);
}

public class AttributeModifier
{
    public required string Id { get; init; }
    public uint Stacks { get; init; } = 1;
    public required IEnumerable<IModifierComponent>? Modifiers { get; init; }
}