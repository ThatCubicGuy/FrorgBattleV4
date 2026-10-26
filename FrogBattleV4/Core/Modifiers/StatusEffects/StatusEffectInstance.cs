using System;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public class StatusEffectInstance : IModifierProvider
{
    private int _turns;
    private int _stacks;
    public required StatusEffectDefinition Definition { get; init; }
    public required EntityUid Applicant { get; init; }
    public required EntityUid Holder { get; init; }

    public int Turns
    {
        get => _turns;
        set => _turns = Math.Clamp(value, 0, Definition.MaxDuration);
    }

    public int Stacks
    {
        get => _stacks;
        set => _stacks = Math.Clamp(value, 0, Definition.MaxStacks);
    }

    [Pure]
    public bool ShouldRemove() => Stacks <= 0 || (Turns <= 0 && !Definition.Properties.HasFlag(EffectFlags.Infinite));

    public void OnTick(TurnMoment moment, CommandBuilder builder)
    {
        if (moment == (Definition.Properties.HasFlag(EffectFlags.StartTick) ? TurnMoment.Start : TurnMoment.End))
        {
            Turns -= 1;
        }
        foreach (var mutator in Definition.Mutators)
        {
            mutator.Tick(moment, this, builder);
        }
    }

    [Pure]
    public ModifierStack GetContributingModifiers(IQuery query, BattleEnvironment env, ModifierContext ctx)
    {
        return Definition.ModifierRules.GetContributingModifiers(query, env, ctx) * Stacks;
    }
}