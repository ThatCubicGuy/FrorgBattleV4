using System;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public class StatusEffectInstance : IModifierProvider
{
    private bool _justAdded = true;
    private int _turns;
    private int _stacks;
    public required StatusEffectId Id { get; init; }
    public required StatusEffect Definition { get; init; }
    public required EntityUid Applicant { get; init; }
    public required EntityUid Holder { get; init; }

    public bool IsBuff => Definition.Type == StatusEffectType.Buff;
    public bool IsDebuff => Definition.Type == StatusEffectType.Debuff;

    public int Turns
    {
        get => _turns;
        set => _turns = Definition.MaxDuration.HasValue
            ? Math.Clamp(value, 0, Definition.MaxDuration.Value)
            : Math.Max(value, 0);
    }

    public int Stacks
    {
        get => _stacks;
        set => _stacks = Math.Clamp(value, 0, Definition.MaxStacks);
    }

    [Pure]
    public bool ShouldRemove() => Stacks <= 0 || (Turns <= 0 && !Definition.Properties.HasFlag(StatusEffectFlags.Infinite));

    public void OnTick(TurnMoment moment, BattleEnvironment env, CommandBuilder builder)
    {
        foreach (var mutator in Definition.Mutators)
        {
            mutator.Tick(moment, this, env, builder);
        }

        if (_justAdded)
        {
            _justAdded = false;
            return;
        }

        if (moment == (Definition.Properties.HasFlag(StatusEffectFlags.StartTick) ? TurnMoment.Start : TurnMoment.End))
        {
            Turns -= 1;
        }
    }

    [Pure]
    public ModifierStack GetContributingModifiers(IQuery query, BattleEnvironment env, in ModifierContext ctx)
    {
        return Definition.Modifiers.GetContributingModifiers(query, env, ctx) * Stacks;
    }
}