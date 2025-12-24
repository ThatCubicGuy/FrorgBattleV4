#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.Pipelines;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem.Components;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;
using FrogBattleV4.Core.EffectSystem.PassiveEffects;
using FrogBattleV4.Core.Extensions;

namespace FrogBattleV4.Core.CharacterSystem;

public class Character : ICharacter
{
    [NotNull] private readonly Dictionary<string, IPoolComponent> _pools;

    public Character()
    {
        // Base stats for any character
        BaseStats = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
        {
            { nameof(Stat.MaxHp), 400000 },
            { nameof(Stat.MaxMana), 100 },
            { nameof(Stat.MaxEnergy), 120 },
            { nameof(Stat.Atk), 1000 },
            { nameof(Stat.Def), 500 },
            { nameof(Stat.Spd), 100 },
            { nameof(Stat.Dex), 0 },
            { nameof(Stat.CritRate), 0.1 },
            { nameof(Stat.CritDamage), 0.5 },
            { nameof(Stat.HitRateBonus), 0 },
            { nameof(Stat.EffectHitRate), 1 },
            { nameof(Stat.EffectRes), 0 },
            { nameof(Stat.ManaCost), 1 },
            { nameof(Stat.ManaRegen), 1 },
            { nameof(Stat.EnergyRecharge), 1 },
            { nameof(Stat.IncomingHealing), 1 },
            { nameof(Stat.OutgoingHealing), 1 },
            { nameof(Stat.ShieldToughness), 1 },
        };
        
        _pools = new List<IPoolComponent>(3)
        {
            new HealthComponent(this)
            {
                CurrentValue = BaseStats[nameof(Stat.MaxHp)],
                Flags = PoolFlags.UsedForLife
            },
            new ManaComponent(this)
            {
                CurrentValue = BaseStats[nameof(Stat.MaxMana)] / 2,
                Flags = PoolFlags.UsedForCasting
            },
            new EnergyComponent(this)
            {
                CurrentValue = 0,
                Flags = PoolFlags.UsedForBurst
            }
        }.ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);
    }

    public ITurn[]? Turns { get; }
    public IAction[]? Actions { get; }

    #region Pools

    public double Hp
    {
        get => Pools[nameof(Hp)].CurrentValue;
        set => Pools[nameof(Hp)].CurrentValue = value;
    }

    public double Mana
    {
        get => Pools[nameof(Mana)].CurrentValue;
        set => Pools[nameof(Mana)].CurrentValue = value;
    }

    public double Energy
    {
        get => Pools[nameof(Energy)].CurrentValue;
        set => Pools[nameof(Energy)].CurrentValue = value;
    }

    #endregion
    
    [NotNull] public IReadOnlyDictionary<string, double> BaseStats { get; }
    [NotNull] public IReadOnlyDictionary<string, IPoolComponent> Pools => _pools;
    public double BaseActionValue => 10000 / GetStat(nameof(Stat.Spd));
    public TurnContext TurnStatus { get; private set; }

    public List<AbilityDefinition> Abilities { get; init; } = [];
    private List<ActiveEffectInstance> ActiveEffects { get; } = [];
    private List<PassiveEffect> PassiveEffects { get; } = [];
    public IReadOnlyList<IAttributeModifier> AttachedEffects => [..ActiveEffects, ..PassiveEffects];
    private List<ActiveEffectInstance> MarkedForDeathEffects { get; } = [];
    
    /// <summary>
    /// Calculates the final value of a stat, optionally in relation to an enemy.
    /// </summary>
    /// <param name="stat">The name of the stat to calculate.</param>
    /// <param name="target">The enemy against which to calculate the stat. Optional.</param>
    /// <returns>The final value of the stat.</returns>
    public double GetStat(string stat, IHasStats target = null)
    {
        return new StatCalcContext
        {
            Stat = stat,
            Owner = this,
            Target = target as ICharacter
        }.ComputePipeline();
    }

    /// <summary>
    /// Decides whether the character can actually execute an ability and isn't stunned.
    /// </summary>
    /// <param name="ctx">The context in which to decide whether the character can act.</param>
    /// <returns>True if the character can take action, false otherwise.</returns>
    public bool CanTakeAction(BattleContext ctx)
    {
        // There's more, but not for now
        return !_pools.Values.Any(x => x.Flags.HasFlag(PoolFlags.Stuns));
    }

    /// <summary>
    /// Runs effect calculations (such as DoT and general effect turn ticks)
    /// </summary>
    /// <param name="ctx">The context in which to start the turn.</param>
    /// <returns>
    /// True if the character can select and execute an ability,
    /// false if their turn is to be skipped.
    /// </returns>
    public bool StartTurn(BattleContext ctx)
    {
        TurnStatus = new TurnContext
        {
            TurnNumber = ctx.TurnNumber,
            Moment = TurnMoment.Start
        };
        foreach (var mutator in ActiveEffects.SelectMany(x => x.Definition.Mutators))
        {
            mutator.OnTurnStart(TurnStatus);
        }
        MarkedForDeathEffects.AddRange(ActiveEffects.Where(x => x.Expire(TurnStatus)));
        return CanTakeAction(ctx);
    }
    
    public void ExecuteAbility(AbilityExecContext ctx)
    {
        TurnStatus = TurnStatus with
        {
            Moment = TurnMoment.Animation
        };
        throw new NotImplementedException();
    }

    public void EndTurn()
    {
        TurnStatus = TurnStatus with
        {
            Moment = TurnMoment.End
        };
        foreach (var mutator in ActiveEffects.SelectMany(x => x.Definition.Mutators))
        {
            mutator.OnTurnEnd(TurnStatus);
        }
        foreach (var effect in MarkedForDeathEffects.Where(effect => effect.TryRemove()))
        {
            ActiveEffects.Remove(effect);
        }
        MarkedForDeathEffects.Clear();
        TurnStatus = TurnStatus with
        {
            Moment = TurnMoment.None
        };
    }
    
    // Effects
    public bool ApplyEffect(EffectApplicationContext effect)
    {
        // Raise some events around here ngl
        if (!effect.CanApply()) return false;
        
        try
        {
            var item = ActiveEffects.First(x => x.Definition == effect.Definition);
            item.Stacks += effect.InitialStacks;
            item.Turns = effect.InitialTurns;
        }
        catch (InvalidOperationException)
        {
            ActiveEffects.Add(new ActiveEffectInstance(effect));
        }

        return true;
    }

    public bool RemoveEffect(ActiveEffectDefinition effect)
    {
        var item = ActiveEffects.First(x => x.Definition == effect);
        return ActiveEffects.Remove(item);
    }

    public void TakeDamage(DamageCalcContext ctx)
    {
        var pool = Pools.Values.LastOrDefault(x => x.Flags.HasFlag(PoolFlags.AbsorbsDamage)) ??
               Pools.Values.LastOrDefault(x => x.Flags.HasFlag(PoolFlags.UsedForLife)) ?? 
               Pools["hp"]; // Fallback on HP if I forgot to set its flag or smth lmao
        pool.CurrentValue -= ctx.ComputePipeline();
    }
}