#nullable enable
using System.Linq;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem.Components;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.CharacterSystem;

/// <summary>
/// The targetable component of any character.
/// </summary>
/// <param name="owner">The character this instance belongs to.</param>
public class CharacterBody(ICharacter owner) : ITargetable
{
    IBattleMember ITargetable.Owner => Owner;

    public ICharacter Owner { get; } = owner;

    public void TakeDamage(DamageSnapshot dmg)
    {
        var pool = Owner.Pools.Values.LastOrDefault(x => x.Flags.HasFlag(PoolFlags.AbsorbsDamage)) ??
                   Owner.Pools.Values.LastOrDefault(x => x.Flags.HasFlag(PoolFlags.UsedForLife));
        if (pool is null) throw new System.InvalidOperationException("Cannot take damage because no pool supports it");
        pool.ProcessSpend(dmg.Amount, new SpendContext
        {
            Owner = Owner,
            Mode = SpendMode.Commit
        });
    }

    public void TakeHealing(double healing)
    {
        var pool = Owner.Pools.Values.LastOrDefault(x => x.Flags.HasFlag(PoolFlags.AbsorbsHealing)) ??
                   Owner.Pools.Values.LastOrDefault(x => x.Flags.HasFlag(PoolFlags.UsedForLife));
        if (pool is null) throw new System.InvalidOperationException("Cannot take healing because no pool supports it");
        pool.ProcessRegen(healing, Owner);
    }
}