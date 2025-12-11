using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.Pipelines;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem.Components;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;
using FrogBattleV4.Core.EffectSystem.PassiveEffects;
using FrogBattleV4.Core.Extensions;

namespace FrogBattleV4.Core.CharacterSystem;

public class Character : ICharacter
{
    private Dictionary<string, IPoolComponent> _pools;

    public Character()
    {
        _pools = ((IEnumerable<IPoolComponent>)
        [
            new HealthComponent(this)
            {
                CurrentValue = BaseStats[nameof(Stat.MaxHp)]
            },
            new ManaComponent(this)
            {
                CurrentValue = BaseStats[nameof(Stat.MaxMana)] / 2
            },
            new EnergyComponent(this)
            {
                CurrentValue = 0
            }
        ]).ToDictionary(x => x.Id, StringComparer.OrdinalIgnoreCase);
        BaseStats =
            new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
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
    }
    
    // Base stats for any character
    public IReadOnlyDictionary<string, double> BaseStats { get; }
    public IReadOnlyDictionary<string, IPoolComponent> Pools => _pools;
    public double BaseActionValue => 10000 / GetStat(nameof(Stat.Spd));
    public bool InTurn { get; private set; }

    public List<AbilityDefinition> Abilities { get; init; } = [];
    private List<ActiveEffectInstance> ActiveEffects { get; } = [];
    private List<PassiveEffect> PassiveEffects { get; } = [];
    public IReadOnlyList<IAttributeModifier> AttachedEffects => [..ActiveEffects, ..PassiveEffects];
    private List<ActiveEffectInstance> MarkedForDeathEffects { get; } = [];
    public double GetStat(string stat, IHasStats target = null)
    {
        return new StatContext
        {
            Stat = stat,
            Owner = this,
            Target = target as ICharacter
        }.ComputePipeline();
    }

    public void ExecuteAbility(AbilityDefinition ability)
    {
        throw new NotImplementedException();
    }

    // might need to remerge this lmao
    // cuz yea.
    public bool TryEnterTurn(TurnContext ctx)
    {
        return !_pools.ContainsKey("stun");
    }

    public void EnterTurn(TurnContext ctx)
    {
        InTurn = true;
        foreach (var effect in ActiveEffects.SelectMany(x => x.Definition.Mutators))
        {
            effect.OnTurnStart(ctx);
        }

        MarkedForDeathEffects.AddRange(ActiveEffects);
    }
    public void ExitTurn(TurnContext ctx)
    {
        foreach (var effect in ActiveEffects.SelectMany(x => x.Definition.Mutators))
        {
            effect.OnTurnEnd(ctx);
        }
        foreach (var effect in MarkedForDeathEffects)
        {
            ActiveEffects.Remove(effect);
        }
        MarkedForDeathEffects.Clear();
        InTurn = false;
    }
    
    // Effects
    public void ApplyEffect(EffectApplicationContext effect)
    {
        if (!effect.CanApply()) return;
        
        try
        {
            var item = ActiveEffects.First(x => x.Definition == effect.Payload.Definition);
            
        }
        catch (InvalidOperationException)
        {
            ActiveEffects.Add(new ActiveEffectInstance(effect.Payload));
        }
    }

    public bool RemoveEffect(ActiveEffectDefinition effect)
    {
        var item = ActiveEffects.First(x => x.Definition == effect);
        return ActiveEffects.Remove(item);
    }
}