#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.ActiveEffects;

public class ActiveEffectInstance : IAttributeModifier
{
    public ActiveEffectDefinition Definition { get; init; }
    public ICharacter? Source { get; init; }
    public ISupportsEffects Holder { get; init; }
    public uint Turns { get; set; }
    public uint Stacks { get; set; }

    public ActiveEffectInstance(ActiveEffectContext ctx)
    {
        Holder = ctx.Holder;
        Source = ctx.Source;
        Turns = ctx.Turns;
        Stacks = ctx.Stacks;
        Definition = ctx.Definition;
    }

    IReadOnlyList<IModifierComponent> IAttributeModifier.Modifiers => Definition.Modifiers;
}