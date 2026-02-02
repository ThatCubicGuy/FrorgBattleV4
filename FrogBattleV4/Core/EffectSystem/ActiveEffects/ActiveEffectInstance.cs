#nullable enable
using System;
using System.Collections.Generic;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.ActiveEffects;

public class ActiveEffectInstance(ActiveEffectApplicationContext ctx) : IAttributeModifier
{
    public ActiveEffectDefinition Definition { get; init; } = ctx.Definition;
    public BattleMember? EffectSource { get; init; } = ctx.Source;
    public ISupportsEffects Holder { get; init; } = ctx.Target;
    public uint Turns { get; set; } = ctx.InitialTurns;
    public uint Stacks { get; set; } = ctx.InitialStacks;
    uint IAttributeModifier.GetStacks(EffectContext ctx) => Stacks;

    public EffectFlags Props { get; init; } = EffectFlags.None;

    IReadOnlyList<IModifierComponent>? IAttributeModifier.Modifiers => Definition.Modifiers;
    
    /// <summary>
    /// Deducts one of the turns left of the effect.
    /// </summary>
    /// <returns>Returns false if the effect hasn't expired, true otherwise (e.g. if its turns have reached zero).</returns>
    public bool Expire(TurnContext ctx)
    {
        if (Props.HasFlag(EffectFlags.Infinite)) return false;
        
        if (Props.HasFlag(EffectFlags.StartTick))
            return ctx.Moment == TurnMoment.Start && --Turns == 0;
        
        return ctx.Moment == TurnMoment.End && --Turns == 0;
    }

    public bool TryRemove()
    {
        if (Props.HasFlag(EffectFlags.Unremovable)) return false;

        if (Props.HasFlag(EffectFlags.RemoveStack)) return --Stacks == 0;
        
        return true;
    }
}

[Flags] public enum EffectFlags
{
    None = 0,
    Unremovable = 1 << 0,
    Invisible = 1 << 1,
    Infinite = 1 << 2,
    StartTick = 1 << 3,
    RemoveStack = 1 << 4,
}