using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.EffectSystem.Components;
using FrogBattleV4.Core.EffectSystem.Modifiers;
using FrogBattleV4.Core.EffectSystem.StatusEffects;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.CharacterSystem;

public class Character : BattleMember, IDamageable, IHasAbilities
{
    private readonly List<StatusEffectInstance> _markedForDeathEffects = [];

    private readonly Dictionary<StatId, double> _baseStats = new()
    {
        { StatId.MaxHp, 400000 },
        { StatId.MaxMana, 100 },
        { StatId.MaxEnergy, 120 },
        { StatId.Atk, 1000 },
        { StatId.Def, 500 },
        { StatId.Spd, 100 },
        { StatId.Dex, 0 },
        { StatId.CritRate, 0.1 },
        { StatId.CritDamage, 0.5 },
        { StatId.HitRateBonus, 0 },
        { StatId.EffectHitRate, 1 },
        { StatId.EffectRes, 0 },
        { StatId.ManaCost, 1 },
        { StatId.ManaRegen, 1 },
        { StatId.EnergyRecharge, 1 },
        { StatId.IncomingHealing, 1 },
        { StatId.OutgoingHealing, 1 },
        { StatId.ShieldToughness, 1 },
    };

    public Character(string name)
    {
        Name = name;
        AddPool(new CharacterPoolComponent(this)
        {
            Id = PoolId.Hp,
            CurrentValue = BaseStats[StatId.MaxHp],
            Tags = [PoolTag.UsedForLife]
        });
        AddPool(new CharacterPoolComponent(this)
        {
            Id = PoolId.Mana,
            CurrentValue = BaseStats[StatId.MaxMana] / 2,
            Tags = [PoolTag.UsedForSpells]
        });
        AddPool(new CharacterPoolComponent(this)
        {
            Id = PoolId.Energy,
            CurrentValue = 0,
            Tags = [PoolTag.UsedForBurst]
        });

        Parts = new HitboxProfile(new TargetablePart
        {
            Parent = this,
            DamageModifiers =
            [
                new DamageModifier
                {
                    Direction = ModifierDirection.Incoming,
                    ModifierStack = new ModifierStack
                    {
                        MultiplyTotal = 0.9
                    }
                }
            ],
        }, new TargetablePart
        {
            Parent = this,
        }, new TargetablePart
        {
            Parent = this,
            DamageModifiers =
            [
                new DamageModifier
                {
                    Direction = ModifierDirection.Incoming,
                    ModifierStack = new ModifierStack
                    {
                        MultiplyTotal = 1.1
                    }
                }
            ],
        });
        Turns = [new CharacterTurn(this)];
    }

    [NotNull] public IReadOnlyDictionary<StatId, double> BaseStats => _baseStats;

    #region Pools

    public PoolComponent Hp => Pools[PoolId.Hp];

    public PoolComponent Mana => Pools[PoolId.Mana];

    public PoolComponent Energy => Pools[PoolId.Energy];

    #endregion

    // TODO: Needs a revision
    private TurnContext TurnStatus { get; set; }

    public List<AbilityDefinition> Abilities { get; init; } = [];
    IEnumerable<AbilityDefinition> IHasAbilities.Abilities => Abilities;

    public override double GetStat(StatId stat, BattleMember target = null)
    {
        return new StatCalcContext
        {
            Stat = stat,
            Actor = this,
            Other = target
        }.ComputePipeline(BaseStats.GetValueOrDefault(stat));
    }

    /// <summary>
    /// Decides whether the character can actually execute an ability and isn't stunned.
    /// </summary>
    /// <param name="ctx">The context in which to decide whether the character can act.</param>
    /// <returns>True if the character can take action, false otherwise.</returns>
    public bool CanTakeAction(BattleContext ctx)
    {
        // There's more, but not for now
        return !Pools.Values.Any(pc => pc.HasTag(PoolTag.Stuns));
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
        foreach (var mutator in ActiveEffects.SelectMany(se => se.Definition.Mutators))
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
        foreach (var mutator in ActiveEffects.SelectMany(se => se.Definition.Mutators))
        {
            mutator.OnTurnEnd(TurnStatus);
        }

        foreach (var effect in _markedForDeathEffects.Where(effect => effect.TryRemove()))
        {
            ForceRemoveActive(effect);
        }

        _markedForDeathEffects.Clear();
        TurnStatus = TurnStatus with
        {
            Moment = TurnMoment.None
        };
    }
}