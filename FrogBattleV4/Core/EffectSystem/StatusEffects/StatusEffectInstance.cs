#nullable enable
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.EffectSystem.StatusEffects;

public class StatusEffectInstance(StatusEffectApplicationContext ctx) : IModifierProvider
{
    public StatusEffectDefinition Definition { get; init; } = ctx.Definition;
    public ISupportsEffects Holder { get; init; } = ctx.Target;
    public BattleMember? EffectSource { get; init; } = ctx.Source;
    public int Turns { get; set; } = ctx.InitialTurns;
    public int Stacks { get; set; } = ctx.InitialStacks;

    public EffectFlags Props { get; init; } = EffectFlags.None;

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

        if (Props.HasFlag(EffectFlags.RemoveStack)) return --Stacks <= 0;

        return true;
    }

    /// <summary>
    /// Returns the effect context of this ActiveEffect in relation to the target.
    /// </summary>
    /// <param name="ctx"></param>
    /// <returns>An effect context about this active effect instance.</returns>
    [Pure]
    public StatusEffectInstanceContext GetInstance(EffectInfoContext ctx)
    {
        return new StatusEffectInstanceContext
        {
            EffectId = Definition.Id,
            EffectSource = EffectSource,
            Holder = Holder,
            Target = ctx.Other,
            Modifiers = Definition.Modifiers,
            Mutators = Definition.Mutators,
            Stacks = Stacks
        };
    }

    [Pure]
    public ModifierStack GetContributingModifiers<TQuery>(TQuery query, EffectInfoContext ctx)
        where TQuery : struct
    {
        return Definition.Modifiers.Where(mr => mr.AppliesFor(query))
            .Aggregate(new ModifierStack(), (stack, rule) =>
                stack + rule.ModifierStack * Stacks);
    }
}

[Flags]
public enum EffectFlags
{
    None = 0,
    Unremovable = 1 << 0,
    Invisible = 1 << 1,
    Infinite = 1 << 2,
    StartTick = 1 << 3,
    RemoveStack = 1 << 4,
}