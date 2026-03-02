#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.Effects;
using FrogBattleV4.Core.Effects.Modifiers;
using FrogBattleV4.Core.Effects.PassiveEffects;
using FrogBattleV4.Core.Effects.StatusEffects;

namespace FrogBattleV4.Core.Calculation;

public class EffectContainer : IBattleMemberComponent
{
    private readonly List<StatusEffectInstance> _statusEffects = [];
    private readonly List<PassiveEffectDefinition> _passiveEffects = [];
    private readonly List<StatusEffectInstance> _markedForDeathEffects = [];

    public event EventHandler<StatusEffectInstance>? EffectApplySuccess;
    public event EventHandler<ApplyEffectCommand>? EffectApplyFailure;
    public event EventHandler<StatusEffectInstance>? EffectRemoveSuccess;
    public event EventHandler<RemoveEffectCommand>? EffectRemoveFailure;

    public IEnumerable<StatusEffectInstance> StatusEffects => _statusEffects;
    public IEnumerable<PassiveEffectDefinition> PassiveEffects => _passiveEffects;
    public IEnumerable<IModifierProvider> All => StatusEffects.Concat<IModifierProvider>(PassiveEffects);

    public void TickStart()
    {
        var markedForImmediateDeath = new List<StatusEffectInstance>(_statusEffects.Count);

        foreach (var effect in _statusEffects.Where(effect => !effect.Props.HasFlag(EffectFlags.Infinite)))
        {
            if (effect.Props.HasFlag(EffectFlags.StartTick))
            {
                // Can't remove from _statusEffects while enumerating
                markedForImmediateDeath.Add(effect);
            }
            else
            {
                _markedForDeathEffects.Add(effect);
            }
        }

        foreach (var effect in markedForImmediateDeath) _statusEffects.Remove(effect);
    }

    public void TickEnd()
    {
        foreach (var effect in _markedForDeathEffects)
        {
            _statusEffects.Remove(effect);
        }

        _markedForDeathEffects.Clear();
    }

    public bool Apply(ApplyEffectCommand cmd)
    {
        if (!(cmd.Rng.NextDouble() < cmd.ComputeTotalChance()))
        {
            EffectApplyFailure?.Invoke(this, cmd);
            return false;
        }

        var item = _statusEffects.FirstOrDefault(se => se.Definition == cmd.Definition);

        if (item is null)
        {
            item = new StatusEffectInstance(cmd);
            _statusEffects.Add(item);
            _markedForDeathEffects.Remove(item);
            foreach (var mutator in cmd.Definition.Mutators)
            {
                mutator.OnApply(cmd);
            }
        }
        else
        {
            item.Stacks += cmd.AddedStacks;
            item.Turns = cmd.InitialTurns;
            _markedForDeathEffects.Remove(item);
        }

        EffectApplySuccess?.Invoke(this, item);
        return true;
    }

    public bool Remove(RemoveEffectCommand ctx)
    {
        var item = _statusEffects.FirstOrDefault(ctx.Query);
        if (item is null || !(ctx.Rng.NextDouble() < ctx.RemovalChance))
        {
            EffectRemoveFailure?.Invoke(this, ctx);
            return false;
        }

        if (ctx.RemovedStacks > 0 || ctx.RemovedTurns > 0)
        {
            item.Stacks -= ctx.RemovedStacks;
            item.Turns -= ctx.RemovedTurns;
            if (item.ShouldRemove()) _statusEffects.Remove(item);
        }
        else _statusEffects.Remove(item);

        EffectRemoveSuccess?.Invoke(this, item);
        return true;
    }

    public void AddPassive(PassiveEffectDefinition passiveEffect) => _passiveEffects.Add(passiveEffect);
}