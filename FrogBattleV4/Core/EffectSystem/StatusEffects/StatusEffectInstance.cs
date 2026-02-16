#nullable enable
using System;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.EffectSystem.StatusEffects;

public class StatusEffectInstance(StatusEffectApplicationContext ctx) : IModifierProvider
{
    public StatusEffectDefinition Definition { get; } = ctx.Definition;
    public ISupportsEffects Holder { get; } = ctx.Target;
    public BattleMember? EffectSource { get; } = ctx.Source;
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

    [Pure]
    public ModifierStack GetContributingModifiers<TQuery>(ModifierQuery<TQuery> query, ModifierContext ctx)
        where TQuery : struct => Definition.Modifiers.GetContribution(query) * Stacks;
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