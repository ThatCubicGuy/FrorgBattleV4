using System.Collections.Frozen;
using System.Collections.Generic;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core;

public static class Registry
{
    public static readonly FrozenDictionary<StatId, double> BaseCharacterStats = new Dictionary<StatId, double>
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
    }.ToFrozenDictionary();

    public static readonly FrozenDictionary<PoolId, IPoolDefinition> BaseCharacterPools = new List<IPoolDefinition>
    {
        new CharacterPoolDefinition
        {
            Id = PoolId.Hp,
            Tags = [PoolTag.UsedForLife],
            MaxValueStat = StatId.MaxHp,
            InitialPercent = 1,
        },
        new CharacterPoolDefinition
        {
            Id = PoolId.Mana,
            Tags = [PoolTag.UsedForSpells],
            MaxValueStat = StatId.MaxMana,
            InitialPercent = 0.5,
        },
        new CharacterPoolDefinition
        {
            Id = PoolId.Energy,
            Tags = [PoolTag.UsedForBurst],
            MaxValueStat = StatId.MaxEnergy,
        },
    }.ToFrozenDictionary(pd => pd.Id);

    public static void ApplyBaseCharacterPools(IBattleMember character)
    {
        if (!character.Pools.Add(new PoolInitContext
            {
                Definition = new CharacterPoolDefinition
                {
                    Id = PoolId.Hp,
                    Tags = [PoolTag.UsedForLife],
                    MaxValueStat = StatId.MaxHp,
                    InitialPercent = 1,
                },
                Target = character,
            }) ||
            !character.Pools.Add(new PoolInitContext
            {
                Definition = new CharacterPoolDefinition
                {
                    Id = PoolId.Mana,
                    Tags = [PoolTag.UsedForSpells],
                    MaxValueStat = StatId.MaxMana,
                    InitialPercent = 0.5,
                },
                Target = character,
            }) ||
            !character.Pools.Add(new PoolInitContext
            {
                Definition = new CharacterPoolDefinition
                {
                    Id = PoolId.Energy,
                    Tags = [PoolTag.UsedForBurst],
                    MaxValueStat = StatId.MaxEnergy,
                },
                Target = character,
            }))
        {
            throw new System.InvalidOperationException("Pool add failure");
        }
        var ctx = new ModifierContext(character);
        character.Pools[PoolId.Hp]!.CurrentValue = ctx.ComputeStat(StatId.MaxHp);
        character.Pools[PoolId.Mana]!.CurrentValue = ctx.ComputeStat(StatId.MaxMana) / 2;
        character.Pools[PoolId.Energy]!.CurrentValue = 0;
    }
}