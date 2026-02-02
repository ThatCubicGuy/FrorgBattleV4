#nullable enable
using System.Linq;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.CharacterSystem;

/// <summary>
/// The targetable component of any character.
/// </summary>
/// <param name="owner">The character this instance belongs to.</param>
public class CharacterBody(Character owner) : IDamageable
{
    BattleMember IDamageable.Parent => Owner;

    public Character Owner { get; } = owner;

    public void ReceiveDamage(DamageResult dmg)
    {
        MutationRequestBuilder
            .ByAnyFlags([PoolFlags.AbsorbsDamage, PoolFlags.UsedForLife], dmg.Amount, PoolMutationFlags.Immutable)
            .ExecuteMutation(new MutationExecContext
            {
                Holder = Owner
            });
    }

    public void ReceiveHealing(double healing)
    {
        MutationRequestBuilder
            .ByAnyFlags([PoolFlags.AbsorbsDamage, PoolFlags.UsedForLife], healing, PoolMutationFlags.Immutable)
            .ExecuteMutation(new MutationExecContext
            {
                Holder = Owner
            });
    }
}