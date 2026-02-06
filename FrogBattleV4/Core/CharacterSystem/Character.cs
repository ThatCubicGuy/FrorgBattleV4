#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.Pipelines;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Modifiers;
using FrogBattleV4.Core.EffectSystem.StatusEffects;
using FrogBattleV4.Core.EffectSystem.PassiveEffects;

namespace FrogBattleV4.Core.CharacterSystem;

public class Character : BattleMember, ISupportsEffects, IHasAbilities
{
    private readonly List<StatusEffectInstance> _markedForDeathEffects = [];

    public event EventHandler<StatusEffectApplicationContext>? EffectApplySuccess;
    public event EventHandler<StatusEffectApplicationContext>? EffectApplyFailure;
    public event EventHandler<StatusEffectRemovalContext>? EffectRemoveSuccess;
    public event EventHandler<StatusEffectRemovalContext>? EffectRemoveFailure;

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
        }.ToDictionary(pc => pc.Id, StringComparer.OrdinalIgnoreCase);

        Turns = [new CharacterTurn(this)];
        Parts = [new CharacterBody(this)];
    }

    [NotNull] public sealed override IEnumerable<IAction> Turns { get; }

    [NotNull] public sealed override IEnumerable<IDamageable> Parts { get; }

    public sealed override IReadOnlyDictionary<string, double> BaseStats { get; }
    public sealed override IReadOnlyDictionary<string, IPoolComponent> Pools { get; }
    public IDamageable Body => Parts.Single();

    #region Pools

    public double Hp => Pools[nameof(Hp)].CurrentValue;

    public double Mana => Pools[nameof(Mana)].CurrentValue;

    public double Energy => Pools[nameof(Energy)].CurrentValue;

    #endregion

    // TODO: Needs a revision
    private TurnContext TurnStatus { get; set; }

    public List<AbilityDefinition> Abilities { get; init; } = [];
    IEnumerable<AbilityDefinition> IHasAbilities.Abilities => Abilities;

    private List<StatusEffectInstance> ActiveEffects { get; } = [];
    private List<PassiveEffectDefinition> PassiveEffects { get; } = [];

    public IEnumerable<IModifierComponent> AttachedEffects =>
        ActiveEffects.Concat<IModifierComponent>(PassiveEffects);

    /// <summary>
    /// Decides whether the character can actually execute an ability and isn't stunned.
    /// </summary>
    /// <param name="ctx">The context in which to decide whether the character can act.</param>
    /// <returns>True if the character can take action, false otherwise.</returns>
    public bool CanTakeAction(BattleContext ctx)
    {
        // There's more, but not for now
        return !Pools.Values.Any(pc => pc.Flags.HasFlag(PoolFlags.Stuns));
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
        foreach (var mutator in ActiveEffects.SelectMany(se => se.Definition.Mutators ?? []))
        {
            mutator.OnTurnStart(TurnStatus);
        }

        _markedForDeathEffects.AddRange(ActiveEffects.Where(se => se.Expire(TurnStatus)));
    }

    public void FinalizeTurn()
    {
        TurnStatus = TurnStatus with
        {
            Moment = TurnMoment.End
        };
        foreach (var mutator in ActiveEffects.SelectMany(se => se.Definition.Mutators ?? []))
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

    public bool ApplyEffect(StatusEffectApplicationContext ctx)
    {
        if (ctx.ComputeTotalChance() < ctx.Rng.NextDouble())
        {
            EffectApplyFailure?.Invoke(this, ctx);
            return false;
        }

        var item = ActiveEffects.FirstOrDefault(se => se.Definition == ctx.Definition);

        EffectApplySuccess?.Invoke(this, ctx);

        if (item is null)
        {
            ActiveEffects.Add(new StatusEffectInstance(ctx));
            foreach (var mutator in ctx.Definition.Mutators)
            {
                mutator.OnApply(ctx);
            }

            return true;
        }

        item.Stacks += ctx.InitialStacks;
        item.Turns = ctx.InitialTurns;

        return true;
    }

    public bool RemoveEffect(StatusEffectRemovalContext ctx)
    {
        var item = ActiveEffects.First(ctx.Query.Invoke);
        if (ctx.Rng.NextDouble() < ctx.RemovalChance && ActiveEffects.Remove(item))
        {
            EffectRemoveSuccess?.Invoke(this, ctx);
        }

        EffectRemoveFailure?.Invoke(this, ctx);
        return false;
    }
}