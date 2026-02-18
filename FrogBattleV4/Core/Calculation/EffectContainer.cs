#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Modifiers;
using FrogBattleV4.Core.EffectSystem.PassiveEffects;
using FrogBattleV4.Core.EffectSystem.StatusEffects;

namespace FrogBattleV4.Core.Calculation;

public class EffectContainer : IEnumerable<IModifierProvider>
{
    private readonly List<StatusEffectInstance> _statusEffects = [];
    private readonly List<PassiveEffectDefinition> _passiveEffects = [];
    private readonly List<StatusEffectInstance> _markedForDeathEffects = [];

    public event EventHandler<StatusEffectInstance>? EffectApplySuccess;
    public event EventHandler<StatusEffectApplicationContext>? EffectApplyFailure;
    public event EventHandler<StatusEffectInstance>? EffectRemoveSuccess;
    public event EventHandler<StatusEffectRemovalContext>? EffectRemoveFailure;
    
    public IEnumerable<StatusEffectInstance> StatusEffects => _statusEffects;
    public IEnumerable<PassiveEffectDefinition> PassiveEffects => _passiveEffects;

    public void TickStart()
    {
        var markedForImmediateDeath = new List<StatusEffectInstance>();

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

    public bool Apply(StatusEffectApplicationContext ctx)
    {
        if (ctx.ComputeTotalChance() < ctx.Rng.NextDouble())
        {
            EffectApplyFailure?.Invoke(this, ctx);
            return false;
        }

        var item = _statusEffects.FirstOrDefault(se => se.Definition == ctx.Definition);

        if (item is null)
        {
            item = new StatusEffectInstance(ctx);
            _statusEffects.Add(item);
            _markedForDeathEffects.Remove(item);
            foreach (var mutator in ctx.Definition.Mutators)
            {
                mutator.OnApply(ctx);
            }
        }
        else
        {
            item.Stacks += ctx.AddedStacks;
            item.Turns = ctx.InitialTurns;
        }

        EffectApplySuccess?.Invoke(this, item);
        return true;
    }

    public bool Remove(StatusEffectRemovalContext ctx)
    {
        var item = _statusEffects.FirstOrDefault(ctx.Query.Invoke);
        if (!(ctx.Rng.NextDouble() < ctx.RemovalChance) || item is null)
        {
            EffectRemoveFailure?.Invoke(this, ctx);
            return false;
        }

        if (ctx.RemovedStacks > 0 || ctx.RemovedTurns > 0)
        {
            _statusEffects.Remove(item);
        }
        else
        {
            item.Stacks -= ctx.RemovedStacks;
            item.Turns -= ctx.RemovedTurns;
            if (item.ShouldRemove()) _statusEffects.Remove(item);
        }

        EffectRemoveSuccess?.Invoke(this, item);
        return true;
    }

    public void AddPassive(PassiveEffectDefinition passiveEffect) => _passiveEffects.Add(passiveEffect);
    public void RemovePassive(PassiveEffectDefinition passiveEffect) => _passiveEffects.Remove(passiveEffect);

    public IEnumerator<IModifierProvider> GetEnumerator() =>
        _statusEffects.Concat<IModifierProvider>(_passiveEffects).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}