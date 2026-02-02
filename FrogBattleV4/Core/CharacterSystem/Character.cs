#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.Pipelines;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;
using FrogBattleV4.Core.EffectSystem.PassiveEffects;

namespace FrogBattleV4.Core.CharacterSystem;

public class Character : BattleMember, ISupportsEffects, IHasAbilities
{
    private readonly List<ActiveEffectInstance> _markedForDeathEffects = [];
    public Character(string name) : base(name)
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

        Pools = new List<IPoolComponent>(3)
        {
            new PoolComponent(this)
            {
                Id = nameof(Pool.Hp),
                CurrentValue = BaseStats[nameof(Stat.MaxHp)],
                Flags = PoolFlags.UsedForLife
            },
            new PoolComponent(this)
            {
                Id = nameof(Pool.Mana),
                CurrentValue = BaseStats[nameof(Stat.MaxMana)] / 2,
                Flags = PoolFlags.UsedForCasting
            },
            new PoolComponent(this)
            {
                Id = nameof(Pool.Energy),
                CurrentValue = 0,
                Flags = PoolFlags.UsedForBurst
            }
        }.ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);
        
        Turns[0] = new CharacterTurn(this);
        Parts[0] = new CharacterBody(this);
    }

    public sealed override IAction[] Turns { get; } = new IAction[4];
    public sealed override IDamageable[] Parts { get; } = new IDamageable[1];
    public sealed override IReadOnlyDictionary<string, double> BaseStats { get; }
    public sealed override IReadOnlyDictionary<string, IPoolComponent> Pools { get; }
    public IDamageable Body => Parts.Single();

    #region Pools

    public double Hp => Pools[nameof(Hp)].CurrentValue;

    public double Mana => Pools[nameof(Mana)].CurrentValue;

    public double Energy => Pools[nameof(Energy)].CurrentValue;

    #endregion
    
    // Needs a revision
    private TurnContext TurnStatus { get; set; }

    public List<AbilityDefinition> Abilities { get; init; } = [];
    private List<ActiveEffectInstance> ActiveEffects { get; } = [];
    private List<PassiveEffect> PassiveEffects { get; } = [];
    public IEnumerable<AttributeModifier> AttachedEffects => ActiveEffects.Concat<IAttributeModifier>(PassiveEffects);

    /// <summary>
    /// Calculates the final value of a stat, optionally in relation to an enemy.
    /// </summary>
    /// <param name="stat">The name of the stat to calculate.</param>
    /// <param name="target">The enemy against which to calculate the stat. Optional.</param>
    /// <returns>The final value of the stat.</returns>
    public override double GetStat(string stat, BattleMember? target = null)
    {
        return new StatCalcContext
        {
            Stat = stat,
            Actor = this,
            Other = target
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
        return !Pools.Values.Any(x => x.Flags.HasFlag(PoolFlags.Stuns));
    }

    /// <summary>
    /// Runs effect calculations (such as DoT and general effect turn ticks)
    /// </summary>
    /// <param name="ctx">The context in which to start the turn.</param>
    public void InitTurn(BattleContext ctx)
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
        _markedForDeathEffects.AddRange(ActiveEffects.Where(x => x.Expire(TurnStatus)));
    }

    public void FinalizeTurn()
    {
        TurnStatus = TurnStatus with
        {
            Moment = TurnMoment.End
        };
        foreach (var mutator in ActiveEffects.SelectMany(x => x.Definition.Mutators))
        {
            mutator.OnTurnEnd(TurnStatus);
        }
        foreach (var effect in _markedForDeathEffects.Where(effect => effect.TryRemove()))
        {
            ActiveEffects.Remove(effect);
        }
        _markedForDeathEffects.Clear();
        TurnStatus = TurnStatus with
        {
            Moment = TurnMoment.None
        };
    }
    
    public bool ApplyEffect(ActiveEffectApplicationContext activeEffect)
    {
        // Raise some events around here ngl
        if (!activeEffect.CanApply()) return false;
        
        var item = ActiveEffects.FirstOrDefault(x => x.Definition == activeEffect.Definition);
        if (item is null)
        {
            ActiveEffects.Add(new ActiveEffectInstance(activeEffect));
            return true;
        }

        item.Stacks += activeEffect.InitialStacks;
        item.Turns = activeEffect.InitialTurns;

        return true;
    }

    public bool RemoveEffect(ActiveEffectRemovalContext ctx)
    {
        var item = ActiveEffects.First(x => x.Definition == ctx.Definition);
        return ctx.Rng.NextDouble() < ctx.RemovalChance && ActiveEffects.Remove(item);
    }
}